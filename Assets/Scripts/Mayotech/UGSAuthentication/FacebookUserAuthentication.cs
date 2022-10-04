using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "FacebookSignIn",menuName = "Authentication/FacebookSignIn")]
    public class FacebookUserAuthentication : UserAuthenticationMethod
    {
        [SerializeField] private string accessToken;

        protected override UniTask SpecificSignIn() =>
            AuthenticationService.Instance.SignInWithFacebookAsync(accessToken).AsUniTask();
    }
}