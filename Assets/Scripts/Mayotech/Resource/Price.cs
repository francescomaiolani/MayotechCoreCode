using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mayotech.Resources
{
    [Serializable]
    public class Price
    {
        [SerializeField] private List<ResourceRequired> resourcesRequired;

        public List<ResourceRequired> ResourcesRequired => resourcesRequired;

        public bool CanAfford() => resourcesRequired.All(item => item.LocalResource.CheckAmount(item.RequiredAmount));

        public void PayPrice() => resourcesRequired.ForEach(item => item.LocalResource.Subtract(item.RequiredAmount));
    }
}