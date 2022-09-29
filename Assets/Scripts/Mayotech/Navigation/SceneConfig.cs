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

        private void OnValidate()
        {
            if (keepLoaded && !saveInSceneHistory)
            {
                keepLoaded = false;
            }
        }

        private NavigationManager GetNavigationManager()
        {
            var guid = AssetDatabase.FindAssets("t: NavigationManager")[0];
            if (guid == null) return null;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return AssetDatabase.LoadAssetAtPath<NavigationManager>(path);
        }
        
        [Button("Add Scene", ButtonSizes.Large),  GUIColor(0.4f, 0.8f, 1)]
        public void AddSceneToNavigationManager() => GetNavigationManager()?.AddScene(this);

        [Button("Add Preload Scene", ButtonSizes.Medium)]
        public void AddPreloadSceneToNavigationManager() => GetNavigationManager()?.AddPreloadScene(this);
    }

    public enum SceneType
    {
        Screen,
        Panel,
        Popup
    }
}