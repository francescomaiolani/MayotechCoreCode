using System;
using Mayotech.UGSEconomy.Currency;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    public abstract class Config : ScriptableObject
    {
        [SerializeField, AutoConnect] protected OnConfigFetchedGameEvent onConfigFetched;
        [SerializeField] protected string configKey;

        public string ConfigKey => configKey;

        public void Init() => onConfigFetched.Subscribe(OnConfigFetched);

        private void OnDestroy() => onConfigFetched.Unsubscribe(OnConfigFetched);

        protected void OnConfigFetched(JToken token)
        {
            if (token[ConfigKey] != null)
            {
                Debug.Log($"OnConfig Fetched: key {ConfigKey}, data: {token}");
                DeserializeData(token[ConfigKey]);
            }
            else
                Debug.LogError($"OnConfig Fetched ERROR: key {ConfigKey}, data: {token}, key not found");
        }

        protected abstract void DeserializeData(JToken data);

        [Button("Add to Config Manager", ButtonSizes.Large)]
        public void AddToConfigManager()
        {
            #if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: ConfigManager");
            foreach (var guid in guids)
            {
                var configManager = AssetDatabase.LoadAssetAtPath<ConfigManager>(AssetDatabase.GUIDToAssetPath(guid));
                configManager.AddConfig(this);
            }
            #endif
        }
    }
    
    [Serializable]
    public abstract class Config<T> : Config, IConfig<T>
    {
        [SerializeField] protected T data;

        public virtual T Data
        {
            get => data;
            set => data = value;
        }
    }
}