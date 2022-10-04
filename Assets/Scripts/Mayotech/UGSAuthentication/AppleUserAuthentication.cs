using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "AppleSignIn",menuName = "Authentication/AppleSignIn")]
    public class AppleUserAuthentication : UserAuthenticationMethod
    {
        [SerializeField] private string idToken;

        protected override UniTask SpecificSignIn() =>
            AuthenticationService.Instance.SignInWithAppleAsync(idToken).AsUniTask();
    }
}