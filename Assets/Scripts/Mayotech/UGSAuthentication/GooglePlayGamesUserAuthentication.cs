using System;
using Cysharp.Threading.Tasks;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

namespace Mayotech.UGSAuthentication
{
    [CreateAssetMenu(fileName = "GooglePlayGamesSignIn", menuName = "Authentication/GooglePlayGamesSignIn")]
    public class GooglePlayGamesUserAuthentication : UserAuthenticationMethod
    {
        private UniTask authenticationTask;
        private bool authenticated;

        private void Awake() => PlayGamesPlatform.Activate();

        protected override UniTask SpecificSignIn()
        {
            authenticationTask = UniTask.WaitUntil(() => authenticated = true);
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login with Google Play games successful.");
                    
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true, async authCode =>
                    {
                        // This token serves as an example to be used for SignInWithGooglePlayGames
                        Debug.Log("Authorization code: " + authCode);
                        await AuthenticationService.Instance.SignInWithGooglePlayGamesAsync(authCode).AsUniTask();
                        authenticated = true;
                    });
                }
                else
                {
                    throw new Exception("Failed to retrieve Google play games authorization code");
                }
            });
            return authenticationTask;
        }

        public async UniTask LinkGooglePlayGamesAccount(string authCode)
        {
            try
            {
                await AuthenticationService.Instance.LinkWithGooglePlayGamesAsync(authCode);
                Debug.Log("Link is successful.");
            }
            catch (AuthenticationException ex) when (ex.ErrorCode == AuthenticationErrorCodes.AccountAlreadyLinked)
            {
                // Prompt the player with an error message.
                Debug.LogError("This user is already linked with another account. Log in instead.");
            }

            catch (AuthenticationException ex)
            {
                // Compare error code to AuthenticationErrorCodes
                // Notify the player with the proper error message
                Debug.LogException(ex);
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