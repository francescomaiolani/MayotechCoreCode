using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(fileName = "LocalResource", menuName = "Resource/LocalResource")]
    [Serializable]
    public class LocalResource : ScriptableObject, IResource
    {
        [SerializeField] protected ResourceType resourceType;
        [SerializeField, AutoConnect] protected OnResourceChangedEvent resourceChangedEvent;
        [SerializeField] protected int amount;

        public string ResourceId { get; }
        
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

        public ResourceType ResourceType => resourceType;

        public bool CheckAmount(int requiredAmount) => Amount >= requiredAmount;

        public virtual void Add(int amount) => Amount += amount;

        public virtual void Subtract(int amount) => Amount -= amount;
    }
}
