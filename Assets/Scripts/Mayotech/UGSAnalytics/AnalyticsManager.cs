using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Unity.Services.Analytics;
using UnityEngine;

[CreateAssetMenu(fileName = "AnalyticsManager", menuName = "Manager/AnalyticsManager")]
public class AnalyticsManager : Service
{
    protected IAnalyticsService IAnalyticsService => AnalyticsService.Instance;

    protected List<string> ConsentIdentifiers { get; set; } = new();

    public override void InitService() { }

    public async UniTask InitializeAnalyiticsConsent()
    {
        try
        {
            ConsentIdentifiers = await IAnalyticsService.CheckForRequiredConsents();
        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately.
            Debug.LogException(e);
            switch (e.Reason)
            {
                case ConsentCheckExceptionReason.Unknown:
                    break;
                case ConsentCheckExceptionReason.DeserializationIssue:
                    break;
                case ConsentCheckExceptionReason.NoInternetConnection:
                    break;
                case ConsentCheckExceptionReason.InvalidConsentFlow:
                    break;
                case ConsentCheckExceptionReason.ConsentFlowNotKnown:
                    break;
            }
        }
    }
    
    public void SendEvent()
    {
        //IAnalyticsService.
    }
}