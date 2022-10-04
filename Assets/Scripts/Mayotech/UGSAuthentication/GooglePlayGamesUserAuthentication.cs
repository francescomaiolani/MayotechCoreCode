using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "GooglePlayGamesSignIn",menuName = "Authentication/GooglePlayGamesSignIn")]
    public class GooglePlayGamesUserAuthentication : UserAuthenticationMethod
    {
        [SerializeField] private string authCode;

        protected override UniTask SpecificSignIn() =>
            AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode).AsUniTask();
    }
}