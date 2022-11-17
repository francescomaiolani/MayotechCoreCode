using System;
using UnityEditor;
using UnityEngine;

namespace Mayotech.Navigation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Navigation/SceneData")]
    public class SceneData : ScriptableObject
    {
        [SerializeField] protected string sceneName;
        [SerializeField] protected SceneType sceneType;
        [SerializeField] protected bool keepLoaded;
        [SerializeField] protected bool saveInSceneHistory;

        public string SceneName => sceneName;
        public SceneType SceneType => sceneType;
        public bool KeepLoaded => keepLoaded;
        public bool SaveInSceneHistory => saveInSceneHistory;

        public SceneData Fill(string sceneName, SceneType sceneType, bool keepLoaded, bool saveInSceneHistory)
        {
            this.sceneName = sceneName;
            this.sceneType = sceneType;
            this.keepLoaded = keepLoaded;
            this.saveInSceneHistory = saveInSceneHistory;
            return this;
        }

        private void OnValidate()
        {
            if (keepLoaded && !saveInSceneHistory)
                keepLoaded = false;
            if (navigationManager == null)
                GetNavigationManager();
        }

        private void GetNavigationManager()
        {
#if UNITY_EDITOR
            var guid = AssetDatabase.FindAssets("t: NavigationManager")[0];
            if (guid == null) return;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            navigationManager = AssetDatabase.LoadAssetAtPath<NavigationManager>(path);
#endif
        }

        private NavigationManager navigationManager;

        [ContextMenu("Add Scene")]
        public void AddSceneToNavigationManager() => navigationManager?.AddScene(this);

        [ContextMenu("Add Preload Scene")]
        public void AddPreloadSceneToNavigationManager() => navigationManager?.AddPreloadScene(this);
    }

    public enum SceneType
    {
        Screen,
        Panel,
        Popup
    }
}