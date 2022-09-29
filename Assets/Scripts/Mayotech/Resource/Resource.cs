using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Resource/Resource")]
    [Serializable]
    public class Resource : ScriptableObject
    {
        [SerializeField] protected ResourceType resourceType;
        [SerializeField, AutoConnect] protected OnResourceChangedEvent resourceChangedEvent;
        protected int amount;
    
        public virtual int Amount
        {
            get => amount;
            set
            {
                var oldAmount = amount;
                amount = value;
                resourceChangedEvent?.RaiseEvent(this, value - amount);
            }
        }

        public ResourceType ResourceType => resourceType;

        public bool CheckAmount(int requiredAmount) => Amount >= requiredAmount;

        public virtual void Add(int amount) => Amount += amount;

        public virtual void Subtract(int amount) => Amount -= amount;
    }
}
