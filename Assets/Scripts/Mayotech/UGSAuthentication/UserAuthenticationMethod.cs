using System;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    public abstract class UserAuthenticationMethod : ScriptableObject, ISignInMethod
    {
        [SerializeField] protected GameEvent onPlayerSignedIn;

        public GameEvent OnPlayerSignedIn => onPlayerSignedIn;

        protected abstract UniTask SpecificSignIn();

        public virtual async UniTask SignIn()
        {
            try
            {
                await SpecificSignIn();
                OnPlayerSignedIn?.RaiseEvent();
                Debug.Log("Sign in anonymously succeeded!");
            }
            catch (AuthenticationException ex)
            {
                HandleAuthenticationException(ex);
            }
            catch (RequestFailedException ex)
            {
                HandleAuthenticationException(ex);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        protected void HandleAuthenticationException(AuthenticationException ex)
        {
            switch (ex.ErrorCode)
            {
                case 10000:
                    Debug.Log($"{AuthenticationErrorCodes.ClientInvalidUserState}");
                    break;
                case 10001:
                    Debug.Log($"{AuthenticationErrorCodes.ClientNoActiveSession}");
                    break;
                case 10002:
                    Debug.Log($"{AuthenticationErrorCodes.InvalidParameters}");
                    break;
                case 10003:
                    Debug.Log($"{AuthenticationErrorCodes.AccountAlreadyLinked}");
                    break;
                case 10004:
                    Debug.Log($"{AuthenticationErrorCodes.AccountLinkLimitExceeded}");
                    break;
                case 10005:
                    Debug.Log($"{AuthenticationErrorCodes.ClientUnlinkExternalIdNotFound}");
                    break;
                case 10006:
                    Debug.Log($"{AuthenticationErrorCodes.ClientInvalidProfile}");
                    break;
                case 10007:
                    Debug.Log($"{AuthenticationErrorCodes.InvalidSessionToken}");
                    break;
                default:
                    Debug.Log($"Unspecified authentication error {ex.ErrorCode}");
                    break;
            }
        }

        protected void HandleAuthenticationException(RequestFailedException ex)
        {
            switch (ex.ErrorCode)
            {
                case 0:
                    Debug.Log($"{CommonErrorCodes.Unknown}");
                    break;
                case 1:
                    Debug.Log($"{CommonErrorCodes.TransportError}");
                    break;
                case 2:
                    Debug.Log($"{CommonErrorCodes.Timeout}");
                    break;
                case 3:
                    Debug.Log($"{CommonErrorCodes.ServiceUnavailable}");
                    break;
                case 4:
                    Debug.Log($"{CommonErrorCodes.ApiMissing}");
                    break;
                case 5:
                    Debug.Log($"{CommonErrorCodes.RequestRejected}");
                    break;
                case 50:
                    Debug.Log($"{CommonErrorCodes.TooManyRequests}");
                    break;
                case 51:
                    Debug.Log($"{CommonErrorCodes.InvalidToken}");
                    break;
                case 52:
                    Debug.Log($"{CommonErrorCodes.TokenExpired}");
                    break;
                case 53:
                    Debug.Log($"{CommonErrorCodes.Forbidden}");
                    break;
                case 54:
                    Debug.Log($"{CommonErrorCodes.NotFound}");
                    break;
                case 55:
                    Debug.Log($"{CommonErrorCodes.InvalidRequest}");
                    break;
                default:
                    Debug.Log("Unknown error");
                    break;
            }

            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
}