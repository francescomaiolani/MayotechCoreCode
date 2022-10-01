using System;
using Mayotech.UGSAuthentication;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class SignInAppLoadingOperation : AppLoadingOperation
    {
        private AuthenticationManager authenticationManager => ServiceLocator.Instance.AuthenticationManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                Debug.Log($"Start operation SignInAppLoadingOperation");
                await authenticationManager.SignInAnonymously();
                Status = LoadingOperationStatus.Completed;
                Debug.Log($"End operation SignInAppLoadingOperation");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Status = LoadingOperationStatus.Failed;
            }
        }
    }
}