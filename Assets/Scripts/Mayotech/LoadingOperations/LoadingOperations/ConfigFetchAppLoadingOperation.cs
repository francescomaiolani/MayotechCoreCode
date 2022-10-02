using System;
using Mayotech.UGSConfig;
using Mayotech.UGSEconomy.Currency;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class ConfigFetchAppLoadingOperation : AppLoadingOperation
    {
        private ConfigManager ConfigManager => ServiceLocator.Instance.ConfigManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await ConfigManager.FetchAllConfigs();
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