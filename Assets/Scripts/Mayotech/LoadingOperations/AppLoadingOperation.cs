using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mayotech.AppLoading
{
    [Serializable]
    public abstract class AppLoadingOperation : MonoBehaviour, IOperationLoader
    {
        [SerializeField] protected LoadingOperationStatus status = LoadingOperationStatus.NotStarted;
        [SerializeField, AutoConnect] protected OnLoadingOperationCompletedGameEvent onLoadingOperationCompleted;
        [SerializeField, AutoConnect] protected OnLoadingOperationFailedGameEvent onLoadingOperationFailed;
        [SerializeField] protected List<AppLoadingOperation> dependenciesOperation;

        public LoadingOperationStatus Status
        {
            get => status;
            set
            {
                status = value;
                switch (status)
                {
                    case LoadingOperationStatus.Completed:
                        onLoadingOperationCompleted?.RaiseEvent(this);
                        break;
                    case LoadingOperationStatus.Failed:
                        onLoadingOperationFailed?.RaiseEvent(this);
                        break;
                }
            }
        }

        public bool Completed => Status == LoadingOperationStatus.Completed;

        public UniTask OperationTask { get; private set; }

        public virtual void InitOperation()
        {
            onLoadingOperationCompleted.Subscribe(OnLoadingOperationCompleted);
            OperationTask = UniTask.WaitUntil(() => Completed);
        }

        protected void OnLoadingOperationCompleted(AppLoadingOperation loadingOperation)
        {
            if (dependenciesOperation.Contains(loadingOperation))
                CheckDependencies();
        }

        public void CheckDependencies()
        {
            if (Status != LoadingOperationStatus.NotStarted) return;
            
            if (dependenciesOperation.Count <= 0 || dependenciesOperation.All(item => item.Completed))
                StartOperation();
        }

        public virtual void StartOperation()
        {
            Status = LoadingOperationStatus.Started;
        }

        [Button("Add to initializer", ButtonSizes.Large)]
        public void AddToInitializer()
        {
            FindObjectOfType<AppInitializer>()?.AddAppLoadingOperation(this);
        }
    }
}

public enum LoadingOperationStatus
{
    NotStarted,
    Started,
    Completed,
    Failed
}