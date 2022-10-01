using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mayotech.Resources;
using Newtonsoft.Json;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.UGSResources
{
    [CreateAssetMenu(fileName = "CurrencyManager", menuName = "Manager/CurrencyManager")]
    public sealed class CurrencyManager : Service
    {
        protected IEconomyConfigurationApiClient EconomyClient => EconomyService.Instance.Configuration;
        protected IEconomyPlayerBalancesApiClient EconomyBalances => EconomyService.Instance.PlayerBalances;

        protected Dictionary<string, CurrencyDefinition> currencyDefinitionsDictionary = new();
        protected Dictionary<string, PlayerBalance> PlayerBalancesDictionary { get; set; } = new();

        [SerializeField] protected List<ScriptableCurrency> allCurrencies;
        protected Dictionary<string, ScriptableCurrency> currenciesDictionary = new();

        public override void InitService()
        {
            EconomyBalances.BalanceUpdated += currencyID =>
            {
                Debug.Log($"The currency that was updated was {currencyID}");
            };
            allCurrencies.ForEach(item => item.Init());
            currenciesDictionary.Clear();
            currenciesDictionary = allCurrencies.ToDictionary(item => item.CurrencyId, item => item);
        }

#region Dictionaries Management

        public ScriptableCurrency GetLocalCurrencyResource(string currencyId) =>
            currenciesDictionary.TryGetValue(currencyId, out var currencyLocalResource) ? currencyLocalResource : null;

        protected CurrencyDefinition GetLocalCurrencyDefinition(string currencyId) =>
            currencyDefinitionsDictionary.TryGetValue(currencyId, out var currencyDefinition)
                ? currencyDefinition
                : null;

        protected PlayerBalance GetLocalPlayerBalance(string currencyId, bool autoAdd = false)
        {
            var balance = PlayerBalancesDictionary.TryGetValue(currencyId, out var playerBalance)
                ? playerBalance
                : null;
            if (!autoAdd || balance != null)
                return balance;

            balance = new PlayerBalance(currencyId);
            PlayerBalancesDictionary.Add(currencyId, balance);
            return balance;
        }

        private void UpdateLocalCurrencyBalance(string currencyId, long amount)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId, true);
            currencyBalance.Balance += amount;
        }

#endregion

#region Currencies Definitions

        public async UniTask GetCurrencyDefinitions()
        {
            try
            {
                var currencyDefinitions = await EconomyClient.GetCurrenciesAsync();
                currencyDefinitionsDictionary.Clear();
                currencyDefinitionsDictionary = currencyDefinitions.ToDictionary(item => item.Id, item => item);
                Debug.Log(JsonConvert.SerializeObject(currencyDefinitionsDictionary));
                foreach (var currencyDefinition in currencyDefinitionsDictionary.Values)
                    UpdateLocalCurrencyDefinition(currencyDefinition);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        protected void UpdateLocalCurrencyDefinition(CurrencyDefinition currencyDefinition)
        {
            var localCurrency = GetLocalCurrencyResource(currencyDefinition.Id);
            if (localCurrency != null)
                localCurrency.CurrencyDefinition = currencyDefinition;
            else
                Debug.LogError($"Local currency  {currencyDefinition.Id} not found in dictionary");
        }

        protected async void GetCurrencyDefinition(string currencyId)
        {
            try
            {
                var definition = await EconomyClient.GetCurrencyAsync(currencyId);
                if (definition == null) return;
                var localDefinition = GetLocalCurrencyDefinition(currencyId);
                if (localDefinition == null)
                    currencyDefinitionsDictionary.Add(definition.Id, definition);
                else
                    currencyDefinitionsDictionary[definition.Id] = definition;
                UpdateLocalCurrencyDefinition(definition);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

#endregion

        [ContextMenu("GetAllCurrenciesBalances")]
        public UniTask GetAllCurrenciesBalances() => GetAllCurrenciesBalances(20);

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

                PlayerBalancesDictionary = balances.ToDictionary(item => item.CurrencyId, item => item);
                Debug.Log(JsonConvert.SerializeObject(PlayerBalancesDictionary));
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

        public async UniTask GetSingleCurrencyBalance(string currencyId)
        {
            var currencyDefinition = GetLocalCurrencyDefinition(currencyId);
            if (currencyDefinition != null)
            {
                try
                {
                    var playerBalance = await currencyDefinition.GetPlayerBalanceAsync();
                    SetLocalCurrencyBalance(currencyId, playerBalance);
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

        protected void SetLocalCurrencyBalance(string currencyId, PlayerBalance playerBalance)
        {
            var oldBalance = GetLocalPlayerBalance(currencyId);
            if (oldBalance != null)
                PlayerBalancesDictionary[currencyId] = playerBalance;
            else
                PlayerBalancesDictionary.Add(currencyId, playerBalance);
        }

        public async UniTask SafeSetSingleCurrencyBalance(string currencyId, int newAmount, Action onSuccess = null,
            Action<Exception> onFail = null)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId);
            if (currencyBalance == null) return;
            var options = new SetBalanceOptions { WriteLock = currencyBalance.WriteLock };
            try
            {
                var newBalance = await EconomyBalances.SetBalanceAsync(currencyId, newAmount, options);
                SetLocalCurrencyBalance(currencyId, newBalance);
                onSuccess?.Invoke();
            }
            catch (EconomyException economyException)
            {
                //await GetSingleCurrencyBalance(currencyId);
                //var newBalance = await EconomyBalances.SetBalanceAsync(currencyId, newAmount, options);
                //SetLocalCurrencyBalance(currencyId, newBalance);
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        public async UniTask IncrementSingleCurrencyBalance(string currencyId, int incrementAmount,
            Action onSuccess = null,
            Action<Exception> onFail = null)
        {
            var currencyBalance = GetLocalPlayerBalance(currencyId);
            if (currencyBalance == null) return;
            var options = new IncrementBalanceOptions { WriteLock = currencyBalance.WriteLock };
            try
            {
                var newBalance = await EconomyBalances.IncrementBalanceAsync(currencyId, incrementAmount, options);
                SetLocalCurrencyBalance(currencyId, newBalance);
            }
            catch (EconomyException economyException)
            {
                //await GetSingleCurrencyBalance(currencyId);
                //var newBalance = await EconomyBalances.SetBalanceAsync(currencyId, newAmount, options);
                //SetLocalCurrencyBalance(currencyId, newBalance);
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

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
                SetLocalCurrencyBalance(currencyId, newBalance);
            }
            catch (EconomyException economyException)
            {
                //await GetSingleCurrencyBalance(currencyId);
                //var newBalance = await EconomyBalances.SetBalanceAsync(currencyId, newAmount, options);
                //SetLocalCurrencyBalance(currencyId, newBalance);
                onFail?.Invoke(economyException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onFail?.Invoke(e);
            }
        }

        public void OnPuchaseCompleted(MakeVirtualPurchaseResult result)
        {
            foreach (var currencyPaid in result.Costs.Currency)
                UpdateLocalCurrencyBalance(currencyPaid.Id, currencyPaid.Amount);
            foreach (var currencyPaid in result.Rewards.Currency)
                UpdateLocalCurrencyBalance(currencyPaid.Id, currencyPaid.Amount);
        }

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
    }
}