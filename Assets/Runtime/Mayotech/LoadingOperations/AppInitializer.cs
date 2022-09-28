using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class AppInitializer : MonoBehaviour
{
    [SerializeField] private List<AppLoadingOperation> appLoadingOperations;
    [SerializeField] private OnAppLoadingCompletedGameEvent onAppLoadingCompleted;
    
    public async void InitLoading()
    {
        try
        {
            appLoadingOperations.ForEach(operation => operation.InitOperation());
            var tasks = appLoadingOperations.Select(item => item.OperationTask);
            await UniTask.WhenAll(tasks);
            onAppLoadingCompleted?.RaiseEvent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}