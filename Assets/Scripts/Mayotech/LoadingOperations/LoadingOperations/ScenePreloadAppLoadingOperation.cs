using System;
using Mayotech.Navigation;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class ScenePreloadAppLoadingOperation : AppLoadingOperation
    {
        protected NavigationManager NavigationManager => ServiceLocator.Instance.NavigationManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await NavigationManager.PreloadScenes();
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