using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [Serializable]
    public class ResourceRequired
    {
        [SerializeField] private LocalResource localResource;
        [SerializeField] private int requiredAmount;

        public LocalResource LocalResource => localResource;
        public int RequiredAmount => requiredAmount;
    }
}