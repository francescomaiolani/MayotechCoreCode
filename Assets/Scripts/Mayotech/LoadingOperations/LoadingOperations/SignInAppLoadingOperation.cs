using System;
using Cysharp.Threading.Tasks;
using Mayotech.UGSAuthentication;
using UnityEngine;
using UnityEngine.UI;

namespace Mayotech.AppLoading
{
    public class SignInAppLoadingOperation : AppLoadingOperation
    {
        [SerializeField] private Button guestButton, gpgButton, googleButton;

        private AuthenticationManager authenticationManager => ServiceLocator.Instance.AuthenticationManager;

        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                var currentAuthenticationMethod = authenticationManager.AuthenticationMethod;
                if (currentAuthenticationMethod == AuthenticationMethod.None)
                {
                    await UniTask.WaitUntil(() =>
                        authenticationManager.AuthenticationMethod != AuthenticationMethod.None);
                }

                await authenticationManager.SignIn();
                Status = LoadingOperationStatus.Completed;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Status = LoadingOperationStatus.Failed;
            }
        }

        public void HideButtons()
        {
            guestButton.gameObject.SetActive(false);
            gpgButton.gameObject.SetActive(false);
            googleButton.gameObject.SetActive(false);
        }
    }
}