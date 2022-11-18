using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mayotech.Navigation
{
    public interface INavigationManager
    {
        UniTask PreloadScenes();
        void EnqueueNavigation(NavigationRequest request);
    }

    [CreateAssetMenu(menuName = "Manager/NavigationManager")]
    public class NavigationManager : Service, INavigationManager
    {
        [SerializeField] protected int navigationRequestLimit = 2;
        [TableList] [SerializeField] protected List<SceneData> preloadScenes;
        [TableList] [SerializeField] protected List<SceneData> allScenes;
        [SerializeField, AutoConnect] protected OnSceneLoadedGameEvent onSceneLoadedGameEvent;
        [SerializeField, AutoConnect] protected OnNavigationStartedGameEvent onNavigationStarted;
        [SerializeField, AutoConnect] protected OnNavigationEndedGameEvent onNavigationEnded;

        protected Dictionary<string, SceneData> allScenesDictionary = new();

        protected Queue<NavigationRequest> navigationRequestsQueue = new();
        protected Stack<string> scenesStack = new();

        protected Scene CurrentScene => SceneManager.GetActiveScene();

        protected SceneData GetSceneConfig(string sceneName) =>
            allScenesDictionary.TryGetValue(sceneName, out var sceneConfig) ? sceneConfig : null;

        public override void InitService()
        {
            foreach (var sceneConfig in allScenes)
                allScenesDictionary.TryAdd(sceneConfig.SceneName, sceneConfig);
        }

        public override bool CheckServiceIntegrity()
        {
            return onSceneLoadedGameEvent != null && onNavigationStarted != null && onNavigationEnded != null &&
                   allScenes.All(item => item != null) && preloadScenes.All(item => item != null);
        }

        public UniTask PreloadScenes()
        {
            var operations = preloadScenes
                .Select(scene => SceneManager.LoadSceneAsync(scene.SceneName, LoadSceneMode.Additive).ToUniTask())
                .ToList();
            return UniTask.WhenAll(operations);
        }

        public void EnqueueNavigation(NavigationRequest request)
        {
            if (navigationRequestsQueue.Count >= navigationRequestLimit) return;

            navigationRequestsQueue.Enqueue(request);
            CheckNavigation();
        }

        protected NavigationRequest PeekNavigationRequest()
        {
            var firstRequest = navigationRequestsQueue.Peek();
            if (firstRequest == null) return null;

            navigationRequestsQueue.Dequeue();
            return firstRequest;
        }

        protected void CheckNavigation()
        {
            var request = PeekNavigationRequest();
            if (request == null) return;
            Navigate(request);
        }

        protected async void Navigate(NavigationRequest request)
        {
            try
            {
                switch (request)
                {
                    case ForwardNavigationRequest forwardNavigationRequest:
                        await ForwardNavigation(forwardNavigationRequest);
                        break;
                    case BackwardNavigationRequest backwardNavigationRequest:
                        await BackwardNavigation(backwardNavigationRequest);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected async UniTask ForwardNavigation(ForwardNavigationRequest forwardNavigationRequest)
        {
            var nextSceneName = forwardNavigationRequest.NextScene;
            if (nextSceneName == CurrentScene.name)
            {
                CheckNavigation();
                return;
            }

            var nextSceneConfig = GetSceneConfig(nextSceneName);
            if (nextSceneConfig == null)
                throw new Exception($"SceneConfig {nextSceneName} not found");

            var currentSceneName = CurrentScene.name;

            var currentSceneConfig = GetSceneConfig(currentSceneName);
            if (currentSceneConfig == null)
                throw new Exception($"SceneConfig {currentSceneName} not found");

            await ForwardNavigationFlow(CurrentScene, nextSceneName);
            if (!currentSceneConfig.KeepLoaded)
                UnloadPreviousScene(currentSceneConfig);

            if (nextSceneConfig.SaveInSceneHistory)
                scenesStack.Push(nextSceneName);
            ActivateScene(nextSceneName, true);
            onNavigationEnded?.RaiseEvent(currentSceneName, nextSceneName);
        }

        protected void UnloadPreviousScene(SceneData currentSceneData)
        {
            SceneManager.UnloadSceneAsync(currentSceneData.SceneName);
        }

        protected async UniTask ForwardNavigationFlow(Scene currentScene, string nextSceneName)
        {
            var nextScene = SceneManager.GetSceneByName(nextSceneName);
            var loadingOperation = UniTask.CompletedTask;
            var sceneLoaded = nextScene.IsValid();
            if (!sceneLoaded)
                loadingOperation = SceneManager.LoadSceneAsync(nextSceneName).ToUniTask();

            onNavigationStarted?.RaiseEvent(currentScene.name, nextSceneName);
            var currentSceneController = CurrentScene.GetSceneController();
            await currentSceneController.NavigateAway();
            await loadingOperation;
            if (!sceneLoaded)
            {
                onSceneLoadedGameEvent?.RaiseEvent(nextSceneName);
                nextScene = SceneManager.GetSceneByName(nextSceneName);
            }

            SceneManager.SetActiveScene(nextScene);
            var nextSceneController = nextScene.GetSceneController();
            nextSceneController.OnSceneLoaded();
            await nextSceneController.NavigateHere();
        }

        protected async UniTask BackwardNavigation(BackwardNavigationRequest backwardNavigationRequest)
        {
            var currentSceneConfig = GetSceneConfig(CurrentScene.name);
            if (currentSceneConfig == null)
                throw new Exception($"SceneConfig {CurrentScene.name} not found");

            if (currentSceneConfig.SaveInSceneHistory)
            {
                scenesStack.Pop();
            }

            if (currentSceneConfig.KeepLoaded)
            {
                ActivateScene(currentSceneConfig.SceneName, false);
            }
            else
            {
                //onNavigationStarted?.RaiseEvent();
            }
        }

        protected void ActivateScene(string sceneName, bool activate)
        {
            var rootObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                root.SetActive(activate);
            }
        }

        public void AddScene(SceneData scene)
        {
            if (!allScenes.Contains(scene))
            {
                allScenes.Add(scene);
                Debug.Log($"Scene {scene.SceneName} added");
            }
            else
                Debug.LogError($"Scene {scene.SceneName} already added!");
        }

        public void AddPreloadScene(SceneData scene)
        {
            if (!preloadScenes.Contains(scene))
            {
                preloadScenes.Add(scene);
                Debug.Log($"Scene {scene.SceneName} added");
            }
            else
                Debug.LogError($"Scene {scene.SceneName} already added!");
        }
    }
}