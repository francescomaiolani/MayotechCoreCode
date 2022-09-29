using Mayotech.AppLoading;
using Unity.Services.Core;
using UnityEngine;

public class UGSInitializerAppLoadingOperation : AppLoadingOperation
{
    public override async void StartOperation()
    {
        base.StartOperation();
        Debug.Log($"Start operation UnityServicesInitializerAppLoadingOperation");
        // UnityServices.InitializeAsync() will initialize all services that are subscribed to Core.
        await UnityServices.InitializeAsync();
        Debug.Log($"UGS State: {UnityServices.State}");
        Status = UnityServices.State switch
        {
            ServicesInitializationState.Uninitialized => LoadingOperationStatus.Failed,
            ServicesInitializationState.Initializing => LoadingOperationStatus.Failed,
            ServicesInitializationState.Initialized => LoadingOperationStatus.Completed
        };

    }
}