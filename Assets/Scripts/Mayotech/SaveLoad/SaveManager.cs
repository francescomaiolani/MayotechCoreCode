using System;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField, AutoConnect] private OnLoadCompletedGameEvent onLoadCompleted;
        [SerializeField, AutoConnect] private OnLoadFailedGameEvent onLoadFailed;

#region Save
        
        /// <summary>
        /// Saves n ISaveable objects
        /// </summary>
        /// <param name="saveableObject"> the saveable objects to save </param>
        public async void SaveData( Action onSaveCompleted = null, Action<Exception> onSaveFailed = null, params ISaveable[] saveableObjects)
        {
            var dataDictionary = saveableObjects.ToDictionary<ISaveable, string, object>(
                saveableObject => saveableObject.Key, saveableObject => saveableObject.CollectSaveData());
            Debug.Log($"Saving: {JsonConvert.SerializeObject(dataDictionary)}");
            try
            {
                await Save(dataDictionary, onSaveCompleted, onSaveFailed);
                Debug.Log($"Save Completed");
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Debug.LogError("Save Failed");
            }
        }

        /// <summary>
        /// Asynchronously saves a dictionary with every key associated to a *complex object 
        /// </summary>
        private async UniTask Save(Dictionary<string, object> data, Action onSaveCompleted = null,
            Action<Exception> onSaveFailed = null)
        {
            try
            {
                await CloudSave.ForceSaveAsync(data);
                onSaveCompleted?.Invoke();
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
                onSaveFailed?.Invoke(cloudSaveException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onSaveFailed?.Invoke(e);
            }
        }

#endregion

#region Load

        /// <summary>
        /// Loads the data for n loadable objects 
        /// </summary>
        public async UniTask LoadData(params ILoadable[] loadableObjects)
        {
            var loadKeys = loadableObjects.Select(loadable => loadable.Key).ToHashSet();
            await Load(loadKeys);
        }

        /// <summary>
        /// Asynchronously loads a dictionary with every key associated to a *complex ILoadable object 
        /// </summary>
        private async UniTask Load(HashSet<string> keys)
        {
            var loadData = new Dictionary<string, string>();
            try
            {
                Debug.Log("Loading Started");
                loadData = await CloudSave.LoadAsync(keys);
                Debug.Log($"Loading Completed: {JsonConvert.SerializeObject(loadData)}");
                onLoadCompleted?.RaiseEvent(loadData);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onLoadFailed?.RaiseEvent(e);
            }
        }

        /// <summary>
        /// Asynchronously loads all player Data 
        /// </summary>
        [ContextMenu("Load All Player Data")]
        public async UniTask LoadAllPlayerData()
        {
            try
            {
                var allData = await CloudSave.LoadAllAsync();
                onLoadCompleted?.RaiseEvent(allData);
                Debug.Log($"Player keys: {JsonConvert.SerializeObject(allData)}");
            }
            catch (CloudSaveException cloudSaveException)
            {
                Debug.LogException(cloudSaveException);
                Debug.LogError($"Reason: {cloudSaveException.Reason}");
                onLoadFailed?.RaiseEvent(cloudSaveException);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                onLoadFailed?.RaiseEvent(e);
            }
        }

        /// <summary>
        /// Asynchronously loads all player saved keys 
        /// </summary>
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

#endregion

        /// <summary>
        /// Deletes player data for a certain key
        /// </summary>
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