using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "GoogleSignIn",menuName = "Authentication/GoogleSignIn")]
    public class GoogleUserAuthentication : UserAuthenticationMethod
    {
        [SerializeField] private string idToken;

        protected override UniTask SpecificSignIn() =>
            AuthenticationService.Instance.SignInWithGoogleAsync(idToken).AsUniTask();
    }
}