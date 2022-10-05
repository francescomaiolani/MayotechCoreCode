using System;
using Mayotech.SaveLoad;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class CloudSaveAppLoadingOperation : AppLoadingOperation
    {
        private SaveManager saveManager => ServiceLocator.Instance.SaveManager;
    
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await saveManager.LoadAllPlayerData();
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

