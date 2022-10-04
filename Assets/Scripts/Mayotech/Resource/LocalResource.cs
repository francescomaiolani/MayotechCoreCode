using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(fileName = "LocalResource", menuName = "Resource/LocalResource")]
    [Serializable]
    public class LocalResource : ScriptableObject, IResource
    {
        [SerializeField] protected string resourceType;
        [SerializeField, AutoConnect] protected OnResourceChangedEvent resourceChangedEvent;
        [SerializeField] protected int amount;
        
        public virtual int Amount
        {
            get => amount;
            set
            {
                var oldAmount = amount;
                amount = value;
                resourceChangedEvent?.RaiseEvent(this, value - oldAmount);
            }
        }

        public string ResourceType => resourceType;
        public string ResourceId => resourceType;

        public bool CheckAmount(int requiredAmount) => Amount >= requiredAmount;

        public virtual void Add(int amount) => Amount += amount;

        public virtual void Subtract(int amount) => Amount -= amount;

        [Button("Add to Resource Manager", ButtonSizes.Large)]
        public void AddToResourceManager()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: ResourceManager");
            var manager = AssetDatabase.LoadAssetAtPath<ResourceManager>(AssetDatabase.GUIDToAssetPath(guids[0]));
            manager.AddResourceToList(this);
#endif
        }
    }
}
