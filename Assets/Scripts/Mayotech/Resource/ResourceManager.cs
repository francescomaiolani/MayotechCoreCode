using System;
using System.Collections.Generic;
using System.Linq;
using Mayotech.SaveLoad;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Manager/ResourceManager")]
    public class ResourceManager : Service, IResourceManager, ISaveable, ILoadable
    {
        [SerializeField, AutoConnect] protected OnLoadCompletedGameEvent onLoadCompleted;
        [SerializeField] protected string saveLoadKey;
        [SerializeField] protected List<LocalResource> resources;

        protected Dictionary<string, LocalResource> resourcesDictionary = new();

        public string Key => saveLoadKey;

        public LocalResource GetResource(string resourceType) =>
            resourcesDictionary.TryGetValue(resourceType, out var value) ? value : null;

        public bool HasResourceInList(LocalResource resource) => resources.Contains(resource);

        public override void InitService()
        {
            resourcesDictionary.Clear();
            resourcesDictionary = resources.ToDictionary(item => item.ResourceType, item => item);
            SubscribeLoad();
        }

        public override bool CheckServiceIntegrity()
        {
            return onLoadCompleted != null && resources.All(item => item != null);
        }

        public void SubscribeLoad() => onLoadCompleted.Subscribe(OnDataLoaded);

        protected void OnDestroy() => onLoadCompleted.Unsubscribe(OnDataLoaded);

        public void OnDataLoaded(Dictionary<string, string> data)
        {
            var hasKey = data.ContainsKey(Key);
            if (!hasKey) return;
            var serializedData = JsonConvert.DeserializeObject<Dictionary<string, int>>(data[Key]);
            foreach (var pair in serializedData)
                GetResource(pair.Key).Amount = pair.Value;
        }

        public void GainResource(string resourceType, int amount)
        {
            GetResource(resourceType)?.Add(amount);
        }

        public bool ConsumeResource(string resourceType, int amount, bool checkResource = true)
        {
            var resource = GetResource(resourceType);
            if (resource == null)
                return false;
            if (!resource.CheckAmount(amount)) return false;
            resource.Subtract(amount);
            return true;
        }

        public bool PayPrice(Price price)
        {
            if (!price.CanAfford()) return false;

            price.PayPrice();
            return true;
        }

        public JObject CollectSaveData()
        {
            var data = new JObject();
            foreach (var pair in resourcesDictionary)
                data[pair.Key] = pair.Value.Amount;
            Debug.Log(data.ToString());
            return data;
        }

        [ContextMenu("Save Resources")]
        public void SaveResources()
        {
            var saveManager = ServiceLocator.Instance.SaveManager;
            saveManager.SaveData(null, null, this);
        }

        [ContextMenu("Load Resources")]
        public async void LoadResource()
        {
            var saveManager = ServiceLocator.Instance.SaveManager;
            await saveManager.LoadData(this);
        }

        public void AddResourceToList(LocalResource localResource)
        {
            if (resources.Contains(localResource))
                Debug.LogError($"Item: {localResource.ResourceType} already added");
            else
            {
                resources.Add(localResource);
                Debug.Log($"Item added {localResource.ResourceType}");
            }
        }

        public void RemoveResourceFromList(LocalResource localResource)
        {
            if (resources.Contains(localResource))
            {
                resources.Remove(localResource);
            }
            else
            {
                Debug.Log($"Item not present in list {localResource.ResourceType}");
            }
        }
    }
}