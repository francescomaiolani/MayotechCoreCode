using System;
using System.Collections;
using System.Collections.Generic;
using Mayotech.AppLoading;
using Mayotech.SaveLoad;
using UnityEngine;

public class CloudSaveAppLoadingOperation : AppLoadingOperation
{
    private SaveManager saveManager => ServiceLocator.Instance.SaveManager;
    
    public override async void StartOperation()
    {
        base.StartOperation();
        try
        {
            await saveManager.LoadAllPlayerData();
            Status = LoadingOperationStatus.Completed;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Status = LoadingOperationStatus.Failed;
        }
    }
}
