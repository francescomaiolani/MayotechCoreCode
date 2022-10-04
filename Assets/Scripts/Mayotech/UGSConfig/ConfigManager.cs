using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unity.Services.RemoteConfig;
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

        public struct userAttributes { }

        public struct appAttributes
        {
            public string appVersion;
        }
    }
}