using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [CreateAssetMenu(fileName = "ConfigManager", menuName = "Manager/ConfigManager")]
    public class ConfigManager : Service
    {
        private RemoteConfigService RemoteConfig => RemoteConfigService.Instance;
        
        [SerializeField] private List<Config> gameConfigs;
        
        public override void InitService() { }

        public async UniTask FetchAllConfigs()
        {
            RemoteConfig.FetchCompleted += OnFetchConfigCompleted; 
            var response = await RemoteConfig.FetchConfigsAsync(new ExampleSample.userAttributes(), new ExampleSample.appAttributes());
            Debug.Log(JsonConvert.SerializeObject(response));
        }

        private void OnFetchConfigCompleted(ConfigResponse response)
        {
            Debug.Log(JsonConvert.SerializeObject(response));
        }
    }
}




