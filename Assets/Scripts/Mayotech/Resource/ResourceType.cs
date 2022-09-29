using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Resource/ResourceType")]
    [Serializable]
    public class ResourceType : ScriptableObject
    {
        [SerializeField] private string type;
        public string Type => type;
    }
}