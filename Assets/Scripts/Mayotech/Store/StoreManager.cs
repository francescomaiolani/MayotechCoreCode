using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Mayotech.UGSResources;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.Store
{
    [CreateAssetMenu(fileName = "StoreManager", menuName = "Manager/StoreManager")]
    public class StoreManager : Service
    {
        private IEconomyPurchasesApiClientApi EconomyPurchase => EconomyService.Instance.Purchases;
        private CurrencyManager CurrencyManager => ServiceLocator.Instance.CurrencyManager;

        public override void InitService() { }

        public void InitPurchasable() { }

        public async UniTask MakeVirtualPurchase(string purchaseID, Action onSuccess, Action<Exception> onFail, List<string> inventoryItemsToPick = null)
        {
            var options = new MakeVirtualPurchaseOptions { PlayersInventoryItemIds = inventoryItemsToPick };
            try
            {
                var purchaseResult = await EconomyPurchase.MakeVirtualPurchaseAsync(purchaseID,
                    inventoryItemsToPick == null ? null : options);
                CurrencyManager.OnPuchaseCompleted(purchaseResult);
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

        public async UniTask RedeemAppleAppStorePurchase(string purchaseId, Action onSuccess, Action onFail)
        {
            var args = new RedeemAppleAppStorePurchaseArgs(purchaseId, "RECEIPT_FROM_APP_STORE", 0, "USD");
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

        private void EvaluateAppStorePurchase(RedeemAppleAppStorePurchaseResult result, Action onSuccess, Action onFail, bool fromException)
        {
            if (fromException)
            {
                onFail?.Invoke();
                return;
            }
        
            switch (result.Verification.Status)
            {
                case AppleVerification.StatusOptions.VALID:
                    onSuccess?.Invoke();
                    break;
                case AppleVerification.StatusOptions.VALIDNOTREDEEMED:
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