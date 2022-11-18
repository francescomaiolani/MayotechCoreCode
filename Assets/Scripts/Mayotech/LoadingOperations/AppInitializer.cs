using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mayotech.Navigation;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mayotech.AppLoading
{
    [Serializable]
    public class AppInitializer : MonoBehaviour
    {
        private NavigationManager NavigationManager => ServiceLocator.Instance.NavigationManager;

        [SerializeField, AutoConnect] private OnAppLoadingCompletedGameEvent onAppLoadingCompleted;
        [SerializeField] private List<AppLoadingOperation> appLoadingOperations;

        private void Start()
        {
            InitLoading();
        }

        public async void InitLoading()
        {
            try
            {
                appLoadingOperations.ForEach(operation => operation?.InitOperation());
                appLoadingOperations.ForEach(operation => operation?.CheckDependencies());
                var tasks = appLoadingOperations.Select(item => item.OperationTask);
                await UniTask.WhenAll(tasks);
                Debug.Log("Loading completed");
                NavigationManager.EnqueueNavigation(new ForwardNavigationRequest("Home"));
                onAppLoadingCompleted?.RaiseEvent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddAppLoadingOperation(AppLoadingOperation operation)
        {
            if (!appLoadingOperations.Contains(operation))
            {
                appLoadingOperations.Add(operation);
                Debug.Log($"{operation.name} added");
            }
            else
                Debug.LogError($"Operation {operation.name} already added");
        }
    }
}