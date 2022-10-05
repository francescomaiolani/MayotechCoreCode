using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(menuName = "Manager/AuthenticationManager")]
    public class AuthenticationManager : Service
    {
        [SerializeField] protected GameEvent onPlayerSignedIn;
        [SerializeField] protected PersistentInt previousAuthenticationMethod;
        [SerializeField,AutoConnect] protected GuestUserAuthentication guestUserAuthentication;
        [SerializeField,AutoConnect] protected FacebookUserAuthentication facebookUserAuthentication;
        [SerializeField,AutoConnect] protected GoogleUserAuthentication googleUserAuthentication;
        [SerializeField,AutoConnect] protected GooglePlayGamesUserAuthentication googlePlayGamesUserAuthentication;
        [SerializeField,AutoConnect] protected AppleUserAuthentication appleUserAuthentication;
        [SerializeField,AutoConnect] protected List<Environment> environments;
        [SerializeField] protected Environment currentEnvironment;

        public Environment CurrentEnvironment => currentEnvironment ?? environments.FirstOrDefault(item =>item.IsDefault);

        protected AuthenticationMethod authenticationMethod;

        public override void InitService()
        {
            authenticationMethod = (AuthenticationMethod)previousAuthenticationMethod.Value;
        }

        private void OnDestroy() => UnsubscribeAuthenticationCallbacks();

        public async UniTask SignIn()
        {
            switch (authenticationMethod)
            {
                case AuthenticationMethod.Facebook:
                    await SignInWithFacebook();
                    break;
                case AuthenticationMethod.Google:
                    await SignInWithGoogle();
                    break;
                case AuthenticationMethod.GooglePlayServices:
                    await SignInWithGooglePlayGames();
                    break;
                case AuthenticationMethod.Apple:
                    await SignInWithApple();
                    break;
                case AuthenticationMethod.None:
                case AuthenticationMethod.Guest:
                default:
                    await SignInAnonymously();
                    break;
            }
        }

        private UniTask SignInAnonymously()
        {
            SubscribeAuthenticationCallbacks();
            return guestUserAuthentication.SignIn();
        }

        private UniTask SignInWithFacebook()
        {
            SubscribeAuthenticationCallbacks();
            return facebookUserAuthentication.SignIn();
        }

        private UniTask SignInWithGoogle()
        {
            SubscribeAuthenticationCallbacks();
            return googleUserAuthentication.SignIn();
        }

        private UniTask SignInWithGooglePlayGames()
        {
            SubscribeAuthenticationCallbacks();
            return googlePlayGamesUserAuthentication.SignIn();
        }

        private UniTask SignInWithApple()
        {
            SubscribeAuthenticationCallbacks();
            return appleUserAuthentication.SignIn();
        }

        private void SubscribeAuthenticationCallbacks()
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
            Debug.Log("Player signed in.");
            onPlayerSignedIn?.RaiseEvent();
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

        [Button("Prod", ButtonSizes.Large)]
        public void SetProdEnvironment()
        {
            currentEnvironment = environments.FirstOrDefault(item => item.name == "production");
        }
        
        [Button("Dev", ButtonSizes.Large)]
        public void SetDevEnvironment()
        {
            currentEnvironment = environments.FirstOrDefault(item => item.name == "development");
        }
    }
}