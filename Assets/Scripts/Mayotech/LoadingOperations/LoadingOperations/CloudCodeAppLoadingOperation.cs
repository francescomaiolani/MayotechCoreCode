using System;
using System.Collections;
using System.Collections.Generic;
using Mayotech.AppLoading;
using Mayotech.CloudCode;
using Mayotech.SaveLoad;
using UnityEngine;

public class CloudCodeAppLoadingOperation : AppLoadingOperation
{
    private CloudCodeManager CloudCodeManager => ServiceLocator.Instance.CloudCodeManager;
    
    public override async void StartOperation()
    {
        base.StartOperation();
        try
        {
            await CloudCodeManager.SendTestRequest();
            Status = LoadingOperationStatus.Completed;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Status = LoadingOperationStatus.Failed;
        }
    }
}
