using Cysharp.Threading.Tasks;

namespace Mayotech.UGSAuthentication
{
    public interface ISignInMethod
    {
        UniTask SignIn();
        GameEvent OnPlayerSignedIn { get; }
    }
}