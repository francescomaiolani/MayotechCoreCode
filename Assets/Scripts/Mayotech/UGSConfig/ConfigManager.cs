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
    /// Config Manager is the class that handles the Config fetch and dispatch to the ScriptableObject of type Config
    /// that represents a specific config of the game. All the configs are stored as a JToken (Dictionary<string, string>)
    /// and injected into the SO with the onConfigFetched GameEvent.
    /// 
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

        /// <summary>
        /// Retrives a specific Config from the dictionary of the ScriptableObjects of type Config
        /// </summary>
        /// <param name="configKey">The key of the config to retrieve</param>
        /// <typeparam name="T">the type of the config</typeparam>
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

        public override bool CheckServiceIntegrity()
        {
            return onConfigFetched != null && gameConfigs.All(item => item != null);
        }

        public async UniTask FetchAllConfigs()
        {
            try
            {
                RemoteConfigService.Instance.FetchCompleted += OnFetchCompleted;
                var userAttributes = new userAttributes();
                var appAttributes = new appAttributes()
                {
                    appVersion = Application.version
                };

                await RemoteConfigService.Instance.FetchConfigsAsync(userAttributes, appAttributes);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in fetching configs: {e.Message}");
            }
        }

        private void OnFetchCompleted(ConfigResponse response)
        {
            switch (response.status)
            {
                case ConfigRequestStatus.None:
                    break;
                case ConfigRequestStatus.Failed:
                    break;
                case ConfigRequestStatus.Success:
                    var configs = response.body[CONFIGS];
                    var settings = configs?[SETTINGS];
                    if (settings == null) return;
                    fetchedConfigs = settings;
                    Debug.Log(JsonConvert.SerializeObject(fetchedConfigs));
                    onConfigFetched?.RaiseEvent(settings);
                    break;
                case ConfigRequestStatus.Pending:
                    break;
            }
        }

        private void OnDestroy() => RemoteConfigService.Instance.FetchCompleted -= OnFetchCompleted;

#region Unity utility methods

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
                AddConfigToList(config);
            }
#endif
        }

        /// <summary>
        /// Adds the config to the list (Editor only)
        /// </summary>
        public void AddConfigToList(Config config)
        {
            if (gameConfigs.Contains(config))
                Debug.LogError($"Resource already added: {config.ConfigKey}");
            else
            {
                gameConfigs.Add(config);
                Debug.Log($"Currency added {config.ConfigKey}");
            }
        }

#endregion
    }
}