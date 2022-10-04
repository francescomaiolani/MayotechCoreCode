using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Mayotech.UGSEconomy.Currency;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sirenix.OdinInspector;
using Unity.Services.RemoteConfig;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    /// <summary>
    /// BUG with unity economy package. Economy configs get pulled even if they shouldn't be taken into consideration
    /// </summary>
    /// 
    [CreateAssetMenu(fileName = "ConfigManager", menuName = "Manager/ConfigManager")]
    public class ConfigManager : Service
    {
        [SerializeField, AutoConnect] protected OnConfigFetchedGameEvent onConfigFetched;
        [SerializeField] protected List<Config> gameConfigs;

        private Dictionary<string, Config> gameConfigDictionary = new();

        protected JToken fetchedConfigs;

        private const string CONFIGS = "configs";
        private const string SETTINGS = "settings";

        public Config<T> GetConfig<T>(string configKey)
        {
            var configFound = gameConfigDictionary.TryGetValue(configKey, out var config) ? config : null;
            if (configFound == null)
                Debug.LogError($"Config {configKey} not found");
            else
            {
                if (config is Config<T> castedConfig)
                    return castedConfig;
                Debug.LogError($"Config {configKey} not castable into {typeof(Config<T>)}");
            }

            return null;
        }

        public override void InitService()
        {
            gameConfigDictionary.Clear();
            gameConfigDictionary = gameConfigs.ToDictionary(item => item.ConfigKey, item => item);
            gameConfigs.ForEach(item => item.Init());
        }

        public async UniTask FetchAllConfigs()
        {
            try
            {
                RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
                var userAttributes = new userAttributes();
                var appAttributes = new appAttributes();
                await RemoteConfigService.Instance.FetchConfigsAsync(userAttributes, appAttributes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in fetching configs: {e.Message}");
            }
        }

        private void OnFetchCompleted(ConfigResponse response)
        {
            var configs = response.body[CONFIGS];
            var settings = configs?[SETTINGS];
            if (settings == null) return;
            fetchedConfigs = settings;
            Debug.Log(JsonConvert.SerializeObject(fetchedConfigs));
            onConfigFetched?.RaiseEvent(settings);
        }

        private void OnDestroy() => RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;

        public void AddConfig(Config config)
        {
            if (!gameConfigs.Contains(config))
            {
                gameConfigs.Add(config);
                Debug.Log($"Config {config.name} added");
            }
            else
                Debug.LogError($"Config {config.name} already added");
        }

        /// <summary>
        /// Adds the configs to the list (Editor only)
        /// </summary>
        [Button("Add all project currencies", ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public void AddAllProjectConfigsToList()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: Config");
            foreach (var guid in guids)
            {
                var config = AssetDatabase.LoadAssetAtPath<Config>(AssetDatabase.GUIDToAssetPath(guid));
                AddConfigsToList(config);
            }
#endif
        }

        /// <summary>
        /// Adds the config to the list (Editor only)
        /// </summary>
        private void AddConfigsToList(Config config)
        {
            if (gameConfigs.Contains(config))
                Debug.LogError($"Resource already added: {config.ConfigKey}");
            else
            {
                gameConfigs.Add(config);
                Debug.Log($"Currency added {config.ConfigKey}");
            }
        }

        public struct userAttributes { }

        public struct appAttributes
        {
            public string appVersion;
        }
    }
}