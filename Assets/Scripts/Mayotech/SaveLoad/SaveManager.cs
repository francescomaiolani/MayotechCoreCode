using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Unity.Services.CloudSave;
using UnityEngine;

namespace Mayotech.SaveLoad
{
    [CreateAssetMenu(menuName = "Manager/SaveManager")]
    public class SaveManager : ScriptableObject
    {
        private ICloudSaveDataClient CloudSave => CloudSaveService.Instance.Data;

        [SerializeField, AutoConnect] private OnSaveCompletedGameEvent onSaveCompleted;
        [SerializeField, AutoConnect] private OnSaveFailedGameEvent onSaveFailed;
        [SerializeField, AutoConnect] private OnLoadCompletedGameEvent onLoadCompleted;
        [SerializeField, AutoConnect] private OnLoadFailedGameEvent onLoadFailed;

        public async void SaveData(string saveKey, ISaveable saveableObject)
        {
            var data = saveableObject.CollectSaveData();
            await Save(saveKey, data);
        }

        public async void SaveData(string saveKey, params Tuple<string, object>[] keyValues)
        {
            var data = keyValues.ToDictionary(keyValue =>
                keyValue.Item1, keyValue => keyValue.Item2);
            await Save(saveKey, data);
        }

        public async void SaveData(string saveKey, params Tuple<StringVariable, object>[] keyValues)
        {
            var data = keyValues.ToDictionary(keyValue =>
                keyValue.Item1.Value, keyValue => keyValue.Item2);
            await Save(saveKey, data);
        }

        public async UniTask Save(string saveKey, Dictionary<string, object> data)
        {
            try
            {
                await CloudSave.ForceSaveAsync(data);
                onSaveCompleted?.RaiseEvent(saveKey);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onSaveFailed?.RaiseEvent(saveKey);
            }
        }
        
        public async UniTask<T> LoadData<T>(string loadKey, HashSet<string> keys, T deserializedObject)
        {
            var data = await Load(loadKey, keys);
            if (data == null) return default;
            var stringifyDictionary = JsonConvert.SerializeObject(data);
            deserializedObject = JsonConvert.DeserializeObject<T>(stringifyDictionary);
            return deserializedObject;
        }

        public async UniTask<Dictionary<string, string>> Load(string loadKey, HashSet<string> keys)
        {
            var savedData = new Dictionary<string, string>();
            try
            {
                savedData = await CloudSave.LoadAsync(keys);
                onLoadCompleted?.RaiseEvent(loadKey);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onLoadFailed?.RaiseEvent(loadKey);
                return null;
            }
            return savedData;
        }

        public void SaveResources()
        {
            var resourceManager = ServiceLocator.Instance.ResourceManager;
            SaveData("resources", resourceManager);
        }
    }
}