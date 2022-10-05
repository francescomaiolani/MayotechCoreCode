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
                await authenticationManager.SignIn();
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