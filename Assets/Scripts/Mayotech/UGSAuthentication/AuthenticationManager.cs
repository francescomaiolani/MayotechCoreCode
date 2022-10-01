using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(menuName = "Manager/AuthenticationManager")]
    public class AuthenticationManager : Service
    {
        [SerializeField] protected AuthenticationMethod authenticationMethod;
        [SerializeField,AutoConnect] private GuestUserAuthentication guestUserAuthentication;

        public override void InitService() { }
    
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