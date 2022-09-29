using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

[CreateAssetMenu(menuName = "Manager/AuthenticationManager")]
public class AuthenticationManager : Service
{
    [SerializeField,AutoConnect] private AnonymousSignIn anonymousSignIn;

    public override void InitService() { }
    
    public UniTask SignInAnonymously()
    {
        SubscribeAuthenticationCallbacks();
        return anonymousSignIn.SignInAnonymouslyAsync();
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
        // Shows how to get a playerID
        Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");
        // Shows how to get an access token
        Debug.Log($"Access Token: {AuthenticationService.Instance.AccessToken}");
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