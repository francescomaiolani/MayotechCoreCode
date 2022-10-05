using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mayotech.UGSEconomy.Currency;
using Mayotech.UGSEconomy.Inventory;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.Store
{
    [CreateAssetMenu(fileName = "StoreManager", menuName = "Manager/StoreManager")]
    public class StoreManager : Service
    {
        private IEconomyPurchasesApiClientApi EconomyPurchase => EconomyService.Instance.Purchases;
        private IEconomyConfigurationApiClient EconomyConfig => EconomyService.Instance.Configuration;
        private CurrencyManager CurrencyManager => ServiceLocator.Instance.CurrencyManager;
        private InventoryManager InventoryManager => ServiceLocator.Instance.InventoryManager;

        private const string CURRENCY = "CURRENCY";
        private const string ITEM = "INVENTORY_ITEM";

        [SerializeField, AutoConnect] protected PurchasesData purchasesData;

        public override void InitService() { }

        public async UniTask InitPurchasables()
        {
            var vitualTask = GetVirtualPurchases();
            var realMoneyTask = GetRealMoneyPurchases();
            try
            {
                await UniTask.WhenAll(vitualTask, realMoneyTask);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async UniTask GetVirtualPurchases()
        {
            try
            {
                var results = await EconomyConfig.GetVirtualPurchasesAsync();
                purchasesData.VirtualPurchases = results.ToDictionary(item => item.Id, item => item);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private async UniTask GetRealMoneyPurchases()
        {
            try
            {
                var results = await EconomyConfig.GetRealMoneyPurchasesAsync();
                purchasesData.RealMoneyPurchases = results.ToDictionary(item => item.Id, item => item);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseID"></param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        /// <param name="inventoryItemsToPick">A list of strings. Defaults to null. The PlayersInventoryItem IDs of the items in
        /// the players inventory that you want to use towards the cost(s) of the purchase.</param>
        public async UniTask MakeVirtualPurchase(string purchaseID, Action onSuccess, Action<Exception> onFail,
            List<string> inventoryItemsToPick = null)
        {
            var options = new MakeVirtualPurchaseOptions { PlayersInventoryItemIds = inventoryItemsToPick };
            try
            {
                var purchaseDefinition = purchasesData.GetVirtualPuchaseDefinition(purchaseID);
                if (purchaseDefinition == null)
                {
                    onFail?.Invoke(new Exception($"Purchase definition {purchaseID} not found"));
                    return;
                }

                var canAfford = CanAffordPurchase(purchaseDefinition);
                if (!canAfford)
                {
                    onFail?.Invoke(new Exception($"Can't afford purchase {purchaseID} locally"));
                    return;
                }
                var purchaseResult = await EconomyPurchase.MakeVirtualPurchaseAsync(purchaseID,
                    inventoryItemsToPick == null ? null : options);
                CurrencyManager.OnPuchaseCompleted(purchaseResult);
                if (purchaseResult.Costs.Inventory.Count > 0 || purchaseResult.Rewards.Inventory.Count > 0)
                {
                    
                }
                InventoryManager.OnPurchaseCompleted(purchaseResult);
                onSuccess?.Invoke();
            }
            catch (EconomyException economyException)
            {
                Debug.LogException(economyException);
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        protected bool CanAffordPurchase(VirtualPurchaseDefinition virtualPurchaseDefinition)
        {
            return virtualPurchaseDefinition.Costs.All(cost =>
            {
                var itemQuantity = cost.Item.GetReferencedConfigurationItem();
                var resourceType = itemQuantity.Type;
                switch (resourceType)
                {
                    case CURRENCY:
                        var currency = CurrencyManager.GetScriptableCurrency(itemQuantity.Id);
                        return (currency?.Amount ?? 0) >= cost.Amount;
                    case ITEM:
                        var item = InventoryManager.GetScriptableItemOfType(itemQuantity.Id);
                        return (item?.Amount ?? 0) >= cost.Amount;
                    default:
                        return false;
                }
            });
        }

        /// <summary>
        /// Redeems a real money purchase by submitting a receipt from the Apple App Store. This is validated and if valid,
        /// the rewards as defined in the configuration are applied to the player’s inventory and currency balances.
        /// </summary>
        /// <param name="purchaseId">The configuration ID of the purchase to make.</param>
        /// <param name="receipt">The receipt data as returned from the Apple App Store.</param>
        /// <param name="localCost">The cost of the purchase as an integer in the minor currency format, for example,
        /// $1.99 USD would be 199.</param>
        /// <param name="currency">ISO-4217 code of the currency used in the purchase.</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        public async UniTask RedeemAppleAppStorePurchase(string purchaseId, string receipt, int localCost,
            string currency, Action onSuccess, Action onFail)
        {
            var args = new RedeemAppleAppStorePurchaseArgs(purchaseId, receipt, localCost, currency);
            try
            {
                var purchaseResult = await EconomyPurchase.RedeemAppleAppStorePurchaseAsync(args);
                EvaluateAppStorePurchase(purchaseResult, onSuccess, onFail, false);
            }
            catch (EconomyAppleAppStorePurchaseFailedException exception)
            {
                Debug.LogException(exception);
                EvaluateAppStorePurchase(exception.Data, onSuccess, onFail, true);
                onFail?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke();
            }
        }

        /// <summary>
        /// Redeems a real money purchase by submitting a receipt from the Google Play Store. This is validated and if
        /// valid, the rewards as defined in the configuration are applied to the player’s inventory and currency balances.
        /// </summary>
        /// <param name="purchaseId">The configuration ID of the purchase to make.</param>
        /// <param name="purchaseData">A JSON encoded string returned from a successful in app billing purchase.</param>
        /// <param name="purchaseDataSignature">A signature of the PurchaseData returned from a successful in app
        /// billing purchase.</param>
        /// <param name="localCost">The cost of the purchase as an integer in the minor currency format, for example,
        /// $1.99 USD would be 199.</param>
        /// <param name="currency"> ISO-4217 code of the currency used in the purchase.</param>
        /// <param name="onSuccess"></param>
        /// <param name="onFail"></param>
        public async UniTask RedeemGooglePlayPurchase(string purchaseId, string purchaseData,
            string purchaseDataSignature, int localCost, string currency, Action onSuccess, Action onFail)
        {
            var args = new RedeemGooglePlayStorePurchaseArgs(purchaseId, purchaseData, purchaseDataSignature, localCost,
                currency);

            try
            {
                var purchaseResult = await EconomyPurchase.RedeemGooglePlayPurchaseAsync(args);
                EvaluatePlayStorePurchase(purchaseResult, onSuccess, onFail, false);
            }
            catch (EconomyGooglePlayStorePurchaseFailedException exception)
            {
                Debug.LogException(exception);
                EvaluatePlayStorePurchase(exception.Data, onSuccess, onFail, true);
                onFail?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke();
            }
        }

        protected void EvaluatePlayStorePurchase(RedeemGooglePlayPurchaseResult result, Action onSuccess,
            Action onFail,
            bool fromException)
        {
            if (fromException)
            {
                onFail?.Invoke();
                return;
            }

            switch (result.Verification.Status)
            {
                case GoogleVerification.StatusOptions.VALID:
                case GoogleVerification.StatusOptions.VALIDNOTREDEEMED:
                    onSuccess?.Invoke();
                    break;
                case GoogleVerification.StatusOptions.INVALIDALREADYREDEEMED:
                case GoogleVerification.StatusOptions.INVALIDVERIFICATIONFAILED:
                case GoogleVerification.StatusOptions.INVALIDANOTHERPLAYER:
                case GoogleVerification.StatusOptions.INVALIDCONFIGURATION:
                case GoogleVerification.StatusOptions.INVALIDPRODUCTIDMISMATCH:
                default:
                    onFail?.Invoke();
                    break;
            }
        }

        protected void EvaluateAppStorePurchase(RedeemAppleAppStorePurchaseResult result, Action onSuccess,
            Action onFail,
            bool fromException)
        {
            if (fromException)
            {
                onFail?.Invoke();
                return;
            }

            switch (result.Verification.Status)
            {
                case AppleVerification.StatusOptions.VALID:
                case AppleVerification.StatusOptions.VALIDNOTREDEEMED:
                    onSuccess?.Invoke();
                    break;
                case AppleVerification.StatusOptions.INVALIDALREADYREDEEMED:
                case AppleVerification.StatusOptions.INVALIDVERIFICATIONFAILED:
                case AppleVerification.StatusOptions.INVALIDANOTHERPLAYER:
                case AppleVerification.StatusOptions.INVALIDCONFIGURATION:
                case AppleVerification.StatusOptions.INVALIDPRODUCTIDMISMATCH:
                default:
                    onFail?.Invoke();
                    break;
            }
        }
    }
}