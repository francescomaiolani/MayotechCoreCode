using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [Serializable]
    public class ResourceRequired
    {
        [SerializeField] private Resource resource;
        [SerializeField] private int requiredAmount;

        public Resource Resource => resource;
        public int RequiredAmount => requiredAmount;
    }
}