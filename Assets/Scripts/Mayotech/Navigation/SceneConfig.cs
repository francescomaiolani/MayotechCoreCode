using System;
using Sirenix.OdinInspector;
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

        [OnInspectorInit]
        private void ValidateInspector()
        {
            if (keepLoaded && !saveInSceneHistory) 
                keepLoaded = false;
            if (navigationManager == null) 
                GetNavigationManager();
            
            inScenes =  navigationManager.allScenes.Contains(this);
            inPreloadScenes = navigationManager.preloadScenes.Contains(this);
        }
        
        private void GetNavigationManager()
        {
            var guid = AssetDatabase.FindAssets("t: NavigationManager")[0];
            if (guid == null) return;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            navigationManager = AssetDatabase.LoadAssetAtPath<NavigationManager>(path);
        }
        
        private NavigationManager navigationManager;
        private bool inScenes, inPreloadScenes;
        
        [InfoBox("Already added to scenes", "inScenes", InfoMessageType = InfoMessageType.Warning)]
        [Button("Add Scene", ButtonSizes.Large),  GUIColor(0.4f, 0.8f, 1)]
        public void AddSceneToNavigationManager() => navigationManager?.AddScene(this);

        [InfoBox("Already added to preload scenes", "inPreloadScenes", InfoMessageType = InfoMessageType.Warning)]
        [Button("Add Preload Scene", ButtonSizes.Large)]
        public void AddPreloadSceneToNavigationManager() => navigationManager?.AddPreloadScene(this);
    }

    public enum SceneType
    {
        Screen,
        Panel,
        Popup
    }
}