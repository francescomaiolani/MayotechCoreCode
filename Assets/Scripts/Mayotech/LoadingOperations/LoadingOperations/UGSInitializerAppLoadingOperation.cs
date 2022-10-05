using Mayotech.AppLoading;
using Mayotech.UGSAuthentication;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class UGSInitializerAppLoadingOperation : AppLoadingOperation
{
    protected AuthenticationManager AuthenticationManager => ServiceLocator.Instance.AuthenticationManager;
    
    public override async void StartOperation()
    {
        base.StartOperation();
        
        var initOptions = new InitializationOptions();
        initOptions.SetEnvironmentName(AuthenticationManager.CurrentEnvironment.EnvironmentName);
        await UnityServices.InitializeAsync(initOptions);
        Debug.Log($"UGS State: {UnityServices.State}");
        Status = UnityServices.State switch
        {
            ServicesInitializationState.Uninitialized => LoadingOperationStatus.Failed,
            ServicesInitializationState.Initializing => LoadingOperationStatus.Failed,
            ServicesInitializationState.Initialized => LoadingOperationStatus.Completed
        };

    }
}