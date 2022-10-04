using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "AnonymousSignIn", menuName = "Authentication/AnonymousSignIn")]
    public class GuestUserAuthentication : UserAuthenticationMethod
    {
        protected override UniTask SpecificSignIn() =>
            AuthenticationService.Instance.SignInAnonymouslyAsync().AsUniTask();
    }
}