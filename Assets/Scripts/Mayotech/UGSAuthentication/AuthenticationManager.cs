using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(menuName = "Manager/AuthenticationManager")]
    public class AuthenticationManager : Service
    {
        [SerializeField] protected PersistentInt previousAuthenticationMethod;
        [SerializeField,AutoConnect] private GuestUserAuthentication guestUserAuthentication;

        private AuthenticationMethod authenticationMethod;

        public override void InitService()
        {
            authenticationMethod = (AuthenticationMethod)previousAuthenticationMethod.Value;
            Debug.Log($"Authentication method: {authenticationMethod}");
        }
        
        public async UniTask SignIn()
        {
            switch (authenticationMethod)
            {
                case AuthenticationMethod.None:
                    break;
                case AuthenticationMethod.Guest:
                    await SignInAnonymously();
                    break;
                case AuthenticationMethod.Facebook:
                    break;
                case AuthenticationMethod.Google:
                    break;
                case AuthenticationMethod.GooglePlayServices:
                    break;
                case AuthenticationMethod.Apple:
                    break;
                default:
                    await SignInAnonymously();
                    break;
            }
        }
    
        public UniTask SignInAnonymously()
        {
            SubscribeAuthenticationCallbacks();
            return guestUserAuthentication.SignIn();
        }
    
        public void SubscribeAuthenticationCallbacks()
        {
            AuthenticationService.Instance.SignedIn += OnPlayerSignedIn;
            AuthenticationService.Instance.SignInFailed += OnPlayerSignedInFailed;
            AuthenticationService.Instance.SignedOut += OnPlayerSignedOut;
            AuthenticationService.Instance.Expired += OnPlayerSessionExpired;
        }
    
        private void UnsubscribeAuthenticationCallbacks()
        {
            AuthenticationService.Instance.SignedIn -= OnPlayerSignedIn;
            AuthenticationService.Instance.SignInFailed -= OnPlayerSignedInFailed;
            AuthenticationService.Instance.SignedOut -= OnPlayerSignedOut;
            AuthenticationService.Instance.Expired -= OnPlayerSessionExpired;
        }

        private void OnPlayerSignedIn()
        {
            
        }

        private void OnPlayerSignedInFailed(RequestFailedException exception)
        {
            Debug.LogError(exception);
        }

        private void OnPlayerSignedOut()
        {
            Debug.Log("Player signed out.");
        }

        private void OnPlayerSessionExpired()
        {
            Debug.Log("Player session could not be refreshed and expired.");
        }
    }
}