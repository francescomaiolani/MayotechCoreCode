using System;
using UnityEditor;
using UnityEngine;

namespace Mayotech.Navigation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Config/SceneConfig")]
    public class SceneConfig : ScriptableObject
    {
        [SerializeField] private string sceneName;
        [SerializeField] private SceneType sceneType;
        [SerializeField] private bool keepLoaded;
        [SerializeField] private bool saveInSceneHistory;
        
        public string SceneName => sceneName;
        public SceneType SceneType => sceneType;
        public bool KeepLoaded => keepLoaded;
        public bool SaveInSceneHistory => saveInSceneHistory;

        private void OnValidate()
        {
            if (keepLoaded && !saveInSceneHistory) 
                keepLoaded = false;
            if (navigationManager == null) 
                GetNavigationManager();
        }
        
        private void GetNavigationManager()
        {
            var guid = AssetDatabase.FindAssets("t: NavigationManager")[0];
            if (guid == null) return;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            navigationManager = AssetDatabase.LoadAssetAtPath<NavigationManager>(path);
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