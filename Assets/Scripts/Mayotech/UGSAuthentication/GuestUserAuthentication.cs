using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(menuName = "Authentication/AnonymousSignIn")]
    public class GuestUserAuthentication : ScriptableObject, ISignInMethod
    {
        [SerializeField] private GameEvent onPlayerSignedIn;
    
        public async UniTask SignIn()
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                onPlayerSignedIn?.RaiseEvent();
                Debug.Log("Sign in anonymously succeeded!");
            }
            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                switch (ex.ErrorCode)
                {
                    case 10000 :
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
            catch (RequestFailedException ex)
            {
                // Compare error code to CommonErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
            }
        }
    }
}
