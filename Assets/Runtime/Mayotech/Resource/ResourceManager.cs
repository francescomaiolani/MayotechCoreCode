using System.Collections.Generic;
using UnityEngine;

namespace Mayotech.Resources
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private List<Resource> resources;
        private Dictionary<ResourceType, Resource> resourcesDictionary = new();

        public Resource GetResource(ResourceType resourceType) =>
            resourcesDictionary.TryGetValue(resourceType, out var value) ? value : null;

        public void Init()
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
    }
}