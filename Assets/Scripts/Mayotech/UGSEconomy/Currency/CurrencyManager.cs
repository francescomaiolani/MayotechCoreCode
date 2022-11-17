using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSEconomy.Currency
{
    [CreateAssetMenu(fileName = "CurrencyManager", menuName = "Manager/CurrencyManager")]
    public class CurrencyManager : Service
    {
        protected IEconomyConfigurationApiClient EconomyClient => EconomyService.Instance.Configuration;
        protected IEconomyPlayerBalancesApiClient EconomyBalances => EconomyService.Instance.PlayerBalances;

        [SerializeField] protected List<ScriptableCurrency> allCurrencies;
        protected Dictionary<string, ScriptableCurrency> currenciesDictionary = new();

        public override void InitService()
        {
            currenciesDictionary.Clear();
            currenciesDictionary = allCurrencies.ToDictionary(item => item.CurrencyId, item => item);
        }

#region Utility Methods

        /// <summary>
        /// Get local scriptable currency with id
        /// </summary>
        public ScriptableCurrency GetScriptableCurrency(string currencyId) =>
            currenciesDictionary.TryGetValue(currencyId, out var currencyLocalResource) ? currencyLocalResource : null;

        /// <summary>
        /// Get the local definition from the scriptable object of the currency
        /// </summary>
        protected CurrencyDefinition GetScriptableCurrencyDefinition(string currencyId)
        {
            var scriptableCurrency = GetScriptableCurrency(currencyId);
            if (scriptableCurrency != null)
                return scriptableCurrency.CurrencyDefinition;
            Debug.LogError($"ScriptableCurrency {currencyId} not found");
            return null;
        }

        /// <summary>
        /// Get the balance from a local currency
        /// </summary>
        protected PlayerBalance GetLocalPlayerBalance(string currencyId)
        {
            var scriptableCurrency = GetScriptableCurrency(currencyId);
            if (scriptableCurrency != null)
                return scriptableCurrency.CurrencyBalance;
            Debug.LogError($"ScriptableCurrency {currencyId} not found");
            return null;
        }

        /// <summary>
        /// Updates the local scriptable of the currency of the specified amount
        /// </summary>
        private void UpdateLocalCurrencyBalance(string currencyId, long amount)
        {
            var scriptableCurrency = GetScriptableCurrency(currencyId);
            if (scriptableCurrency != null)
                scriptableCurrency.UpdateCurrencyAmount(amount);
            else
                Debug.LogError($"Currency balance is null: {currencyId}");
        }

        /// <summary>
        /// Locally assigns the definition to the scriptable object representing the currency
        /// </summary>
        protected void AssignCurrencyDefinition(CurrencyDefinition currencyDefinition)
        {
            var scriptableCurrency = GetScriptableCurrency(currencyDefinition.Id);
            if (scriptableCurrency != null)
                scriptableCurrency.CurrencyDefinition = currencyDefinition;
            else
                Debug.LogError($"ScriptableItem {currencyDefinition.Id} not found");
        }

        /// <summary>
        /// Assign a new item instance to the associated scriptable
        /// </summary>
        /// <param name="inventoryItem"> the new item to assign </param>
        public void AssignCurrencyBalance(PlayerBalance playerBalance)
        {
            var scriptableCurrency = GetScriptableCurrency(playerBalance.CurrencyId);
            if (scriptableCurrency != null)
                scriptableCurrency.CurrencyBalance = playerBalance;
            else
                Debug.LogError($"ScriptableItem {scriptableCurrency.CurrencyId} not found");
        }

#endregion

#region Currencies Definitions

        /// <summary>
        /// Gets all the currencies definitions and assign them to te scriptable currencies
        /// </summary>
        public async UniTask GetCurrencyDefinitions()
        {
            try
            {
                var currencyDefinitions = await EconomyClient.GetCurrenciesAsync();
                foreach (var definition in currencyDefinitions)
                    AssignCurrencyDefinition(definition);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Gets a specific currency definition and assign it to te scriptable currency
        /// </summary>
        protected async UniTask GetCurrencyDefinition(string currencyId)
        {
            try
            {
                var definition = await EconomyClient.GetCurrencyAsync(currencyId);
                if (definition == null) return;
                AssignCurrencyDefinition(definition);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#endregion

        [ContextMenu("GetAllCurrenciesBalances")]
        public UniTask GetAllCurrenciesBalances() => GetAllCurrenciesBalances(20);

        /// <summary>
        /// Gets the balance of every currency of the player and assign it to the scriptable currency
        /// </summary>
        public async UniTask GetAllCurrenciesBalances(int itemsToFetch = 20)
        {
            var options = new GetBalancesOptions { ItemsPerFetch = itemsToFetch };
            try
            {
                var getBalancesResult = await EconomyBalances.GetBalancesAsync(options);
                var balances = getBalancesResult.Balances;

                while (getBalancesResult.HasNext)
                {
                    getBalancesResult = await getBalancesResult.GetNextAsync(options.ItemsPerFetch);
                    balances = getBalancesResult.Balances;
                }

                foreach (var balance in balances)
                    AssignCurrencyBalance(balance);
            }
            catch (EconomyException economyException)
            {
                Debug.LogException(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Gets the balance of a single currency of the player and assign it to the scriptable currency
        /// </summary>
        public async UniTask GetSingleCurrencyBalance(string currencyId)
        {
            var currencyDefinition = GetScriptableCurrencyDefinition(currencyId);
            if (currencyDefinition != null)
            {
                try
                {
                    var playerBalance = await currencyDefinition.GetPlayerBalanceAsync();
                    AssignCurrencyBalance(playerBalance);
                }
                catch (EconomyException economyException)
                {
                    Debug.LogException(economyException);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }
        }

        /// <summary>
        /// Sets the balance of a currency of the player and assign the new balance to the scriptable currency
        /// </summary>
        public async UniTask SafeSetSingleCurrencyBalance(string currencyId, int newAmount, Action onSuccess = null,
            Action<Exception> onFail = null)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId);
            if (currencyBalance == null) return;
            var options = new SetBalanceOptions { WriteLock = currencyBalance.WriteLock };
            try
            {
                var newBalance = await EconomyBalances.SetBalanceAsync(currencyId, newAmount, options);
                AssignCurrencyBalance(newBalance);
                onSuccess?.Invoke();
            }
            catch (EconomyException economyException)
            {
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        /// <summary>
        /// Increments the balance of a currency of the player and assign the new balance to the scriptable currency
        /// </summary>
        public async UniTask IncrementSingleCurrencyBalance(string currencyId, int incrementAmount,
            Action onSuccess = null, Action<Exception> onFail = null)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId);
            if (currencyBalance == null) return;
            var options = new IncrementBalanceOptions { WriteLock = currencyBalance.WriteLock };
            try
            {
                var newBalance = await EconomyBalances.IncrementBalanceAsync(currencyId, incrementAmount, options);
                AssignCurrencyBalance(newBalance);
                onSuccess?.Invoke();
            }
            catch (EconomyException economyException)
            {
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        /// <summary>
        /// Decrement the balance of a currency of the player and assign the new balance to the scriptable currency
        /// </summary>
        public async UniTask DecrementSingleCurrencyBalance(string currencyId, int decrementAmount,
            Action onSuccess = null,
            Action<Exception> onFail = null)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId);
            if (currencyBalance == null) return;
            var options = new DecrementBalanceOptions { WriteLock = currencyBalance.WriteLock };
            try
            {
                var newBalance = await EconomyBalances.DecrementBalanceAsync(currencyId, decrementAmount, options);
                AssignCurrencyBalance(newBalance);
                onSuccess?.Invoke();
            }
            catch (EconomyException economyException)
            {
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        /// <summary>
        /// Updates the balance of the scriptable currencies after a purchase from the StoreManager
        /// </summary>
        public void OnPuchaseCompleted(MakeVirtualPurchaseResult result)
        {
            foreach (var currencyPaid in result.Costs.Currency)
                UpdateLocalCurrencyBalance(currencyPaid.Id, currencyPaid.Amount);
            foreach (var currencyPaid in result.Rewards.Currency)
                UpdateLocalCurrencyBalance(currencyPaid.Id, currencyPaid.Amount);
        }

        /// <summary>
        /// Adds the currency to the list (Editor only)
        /// </summary>
        public void AddCurrencyToList(ScriptableCurrency resource)
        {
            if (allCurrencies.Contains(resource))
                Debug.LogError($"Resource already added: {resource.CurrencyId}");
            else
            {
                allCurrencies.Add(resource);
                Debug.Log($"Currency added {resource.CurrencyId}");
            }
        }

        /// <summary>
        /// Adds the currency to the list (Editor only)
        /// </summary>
        [Button("Add all project currencies", ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public void AddAllProjectCurrenciesToList()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: ScriptableCurrency");
            foreach (var guid in guids)
            {
                var currency = AssetDatabase.LoadAssetAtPath<ScriptableCurrency>(AssetDatabase.GUIDToAssetPath(guid));
                AddCurrencyToList(currency);
            }
#endif
        }
    }
}