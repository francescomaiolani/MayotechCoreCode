using System;
using Mayotech.CloudCode;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class CloudCodeAppLoadingOperation : AppLoadingOperation
    {
        private CloudCodeManager CloudCodeManager => ServiceLocator.Instance.CloudCodeManager;
    
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await CloudCodeManager.SendTestRequest();
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

