using System;
using Mayotech.AppLoading;
using UnityEngine;

public class InitializeAnalyticsAppLoadingOperation : AppLoadingOperation
{
    protected AnalyticsManager AnalyticsManager => ServiceLocator.Instance.AnalyticsManager;
    
    public override async void StartOperation()
    {
        base.StartOperation();
        try
        {
            await AnalyticsManager.InitializeAnalyiticsConsent();
            Status = LoadingOperationStatus.Completed;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            Status = LoadingOperationStatus.Failed;
        }
    }
}
