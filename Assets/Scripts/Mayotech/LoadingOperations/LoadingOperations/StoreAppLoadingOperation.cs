using System;
using Mayotech.Store;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class StoreAppLoadingOperation : AppLoadingOperation
    {
        protected StoreManager StoreManager => ServiceLocator.Instance.StoreManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await StoreManager.InitPurchasables();
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