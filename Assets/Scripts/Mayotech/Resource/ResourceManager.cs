using System.Collections.Generic;
using System.Linq;
using Mayotech.SaveLoad;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Manager/ResourceManager")]
    public class ResourceManager : Service, ISaveable
    {
        [SerializeField] private List<Resource> resources;
        private Dictionary<ResourceType, Resource> resourcesDictionary = new();

        public Resource GetResource(ResourceType resourceType) =>
            resourcesDictionary.TryGetValue(resourceType, out var value) ? value : null;

        public override void InitService()
        {
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

        public Dictionary<string, object> CollectSaveData()
        {
            return resourcesDictionary.ToDictionary(keyValue => keyValue.Key.Type,
                keyValue => (object)keyValue.Value.Amount);
        }
    }
}