using System;
using Mayotech.Navigation;
using Mayotech.SafeTime;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class SafeTimeAppLoadingOperation : AppLoadingOperation
    {
        protected TimeManager TimeManager => ServiceLocator.Instance.TimeManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await TimeManager.GetServerTime();
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