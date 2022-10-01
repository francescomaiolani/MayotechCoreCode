using System.Collections.Generic;
using System.Linq;
using Mayotech.SaveLoad;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Manager/ResourceManager")]
    public class ResourceManager : Service, ISaveable, ILoadable
    {
        [SerializeField] private List<LocalResource> resources;
        private Dictionary<ResourceType, LocalResource> resourcesDictionary = new();

        public LocalResource GetResource(ResourceType resourceType) =>
            resourcesDictionary.TryGetValue(resourceType, out var value) ? value : null;

        public override void InitService()
        {
            Debug.Log("Init resource");
            resources.ForEach(item => resourcesDictionary.Add(item.ResourceType, item));
        }

        public void GainResource(ResourceType resourceType, int amount)
        {
            GetResource(resourceType)?.Add(amount);
        }

        public bool ConsumeResource(ResourceType resourceType, int amount, bool checkResource = true)
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
        
        public Dictionary<string, object> LoadedObject { get; set; }
        
        public Dictionary<string, object> CollectSaveData()
        {
            return resourcesDictionary.ToDictionary(keyValue => keyValue.Key.Type,
                keyValue => (object)keyValue.Value.Amount);
        }
        
        public HashSet<string> CollectLoadData()
        {
            return new HashSet<string>(resourcesDictionary.Keys.Select(item => item.Type));
        }

        [ContextMenu("Save Resources")]
        public void SaveResources()
        {
            var saveManager = ServiceLocator.Instance.SaveManager;
            saveManager.SaveData("resources", this);
        }

        [ContextMenu("Load Resources")]
        public void LoadResource()
        { 
            var saveManager = ServiceLocator.Instance.SaveManager;
            saveManager.LoadData("resources",this, LoadedObject);
        }
    }
}