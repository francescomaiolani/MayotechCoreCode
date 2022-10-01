using System;
using Mayotech.UGSEconomy.Currency;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class CurrencyAppLoadingOperation : AppLoadingOperation
    {
        private CurrencyManager CurrencyManager => ServiceLocator.Instance.CurrencyManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await CurrencyManager.GetCurrencyDefinitions();
                await CurrencyManager.GetAllCurrenciesBalances();
                Status = LoadingOperationStatus.Completed;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Status = LoadingOperationStatus.Failed;
            }
        }
    }
}