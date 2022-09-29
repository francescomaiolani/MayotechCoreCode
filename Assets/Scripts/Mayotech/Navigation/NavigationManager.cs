using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mayotech.Navigation
{
    [CreateAssetMenu(menuName = "Manager/NavigationManager")]
    public class NavigationManager : Service
    {
        [SerializeField] private int navigationRequestLimit = 2;
        [SerializeField] private List<SceneConfig> preloadScenes;
        [SerializeField] private List<SceneConfig> allScenes;
        [SerializeField, AutoConnect] private OnSceneLoadedGameEvent onSceneLoadedGameEvent;
        [SerializeField, AutoConnect] private OnNavigationStartedGameEvent onNavigationStarted;
        [SerializeField, AutoConnect] private OnNavigationEndedGameEvent onNavigationEnded;

        private Dictionary<string, SceneConfig> allScenesDictionary = new();

        private Queue<NavigationRequest> navigationRequestsQueue = new();
        private Stack<string> scenesStack = new();

        private Scene CurrentScene => SceneManager.GetActiveScene();

        private SceneConfig GetSceneConfig(string sceneName) =>
            allScenesDictionary.TryGetValue(sceneName, out var sceneConfig) ? sceneConfig : null;
        
        public override void InitService()
        {
            foreach (var sceneConfig in allScenes)
            {
                allScenesDictionary.TryAdd(sceneConfig.SceneName, sceneConfig);
            }
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

        public NavigationRequest PeekNavigationRequest()
        {
            var firstRequest = navigationRequestsQueue.Peek();
            if (firstRequest == null) return null;

            navigationRequestsQueue.Dequeue();
            return firstRequest;
        }

        private void CheckNavigation()
        {
            var request = PeekNavigationRequest();
            if (request == null) return;
            Navigate(request);
        }

        private async void Navigate(NavigationRequest request)
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

        private async UniTask ForwardNavigation(ForwardNavigationRequest forwardNavigationRequest)
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

        private void UnloadPreviousScene(SceneConfig currentSceneConfig)
        {
            SceneManager.UnloadSceneAsync(currentSceneConfig.SceneName);
        }

        private async UniTask ForwardNavigationFlow(Scene currentScene, string nextSceneName)
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

        private async UniTask BackwardNavigation(BackwardNavigationRequest backwardNavigationRequest)
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

        private void ActivateScene(string sceneName, bool activate)
        {
            var rootObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();
            foreach (var root in rootObjects)
            {
                root.SetActive(activate);
            }
        }

        public void AddScene(SceneConfig scene)
        {
            if (!allScenes.Contains(scene))
            {
                allScenes.Add(scene);
                Debug.Log($"Scene {scene.SceneName} added");
            }
            else
                Debug.LogError($"Scene {scene.SceneName} already added!");
        }
        
        public void AddPreloadScene(SceneConfig scene)
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