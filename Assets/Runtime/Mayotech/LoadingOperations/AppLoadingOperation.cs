using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public abstract class AppLoadingOperation : IOperationLoader
{
    [SerializeField] protected OnLoadingOperationCompletedGameEvent onLoadingOperationCompleted;
    [SerializeField] protected List<AppLoadingOperation> dependenciesOperation;
    private bool completed;

    public bool Completed
    {
        get => completed;
        set
        {
            completed = value;
            if (completed)
                onLoadingOperationCompleted?.RaiseEvent(this);
        }
    }

    public UniTask OperationTask { get; private set;}

    public virtual void InitOperation()
    {
        OperationTask = UniTask.WaitUntil(()=>Completed);
        if (dependenciesOperation.Count <= 0) 
            StartOperation();
    }

    private void OnLoadingOperationCompleted(AppLoadingOperation loadingOperation)
    {
        if (dependenciesOperation.Contains(loadingOperation)) 
            CheckDependencies();
    }

    private void CheckDependencies()
    {
        if (dependenciesOperation.All(item => item.Completed))
            StartOperation();
    }
    
    public abstract void StartOperation();
}

public class ScenePreloadAppLoadingOperation : AppLoadingOperation
{
    public override async void StartOperation()
    {
        await NavigationManager.Instance.PreloadScenes();
        Completed = true;
    }
}

public interface IOperationLoader
{
    void StartOperation();
    bool Completed { get; set;}
    void InitOperation();
}