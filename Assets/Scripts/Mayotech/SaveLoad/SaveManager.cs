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

#region Save

        public async void SaveData(string saveKey, ISaveable saveableObject)
        {
            Debug.Log("Save started");
            var data = saveableObject.CollectSaveData();
            await Save(saveKey, data);
            Debug.Log("Save Completed");
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

        private async UniTask Save(string saveKey, Dictionary<string, object> data)
        {
            try
            {
                await CloudSave.ForceSaveAsync(data);
                onSaveCompleted?.RaiseEvent(saveKey);
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
                onSaveFailed?.RaiseEvent(saveKey);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onSaveFailed?.RaiseEvent(saveKey);
            }
        }

#endregion

#region Load
        
        public async UniTask<T> LoadData<T>(string loadKey, HashSet<string> keys, T deserializedObject)
        {
            var data = await Load(loadKey, keys);
            if (data == null) return default;
            var stringifyDictionary = JsonConvert.SerializeObject(data);
            deserializedObject = JsonConvert.DeserializeObject<T>(stringifyDictionary);
            return deserializedObject;
        }

        public async UniTask<T> LoadData<T>(string loadKey, ILoadable loadableObject, T deserializedObject)
        {
            var loadKeys = loadableObject.CollectLoadData();
            var data = await Load(loadKey, loadKeys);
            if (data == null) return default;
            var stringifyDictionary = JsonConvert.SerializeObject(data);
            deserializedObject = JsonConvert.DeserializeObject<T>(stringifyDictionary);
            return deserializedObject;
        }

        private async UniTask<Dictionary<string, string>> Load(string loadKey, HashSet<string> keys)
        {
            var savedData = new Dictionary<string, string>();
            try
            {
                Debug.Log("Loading Started");
                savedData = await CloudSave.LoadAsync(keys);
                Debug.Log($"Loading Completed: {JsonConvert.SerializeObject(savedData)}");
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

        [ContextMenu("Load All Player Keys")]
        public async void LoadAllPlayerKeys()
        {
            try
            {
                var keys = await CloudSave.RetrieveAllKeysAsync();
                Debug.Log($"Player keys: {JsonConvert.SerializeObject(keys)}");
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
        [ContextMenu("Load All Player Data")]
        public async void LoadAllPlayerData()
        {
            try
            {
                var allData = await CloudSave.LoadAllAsync();
                Debug.Log($"Player keys: {JsonConvert.SerializeObject(allData)}");
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
        
#endregion
        
        public async void DeleteData(string key)
        {
            try
            {
                await CloudSave.ForceDeleteAsync(key);
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}