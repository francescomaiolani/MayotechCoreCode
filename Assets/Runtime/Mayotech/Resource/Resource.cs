using System;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Config/Resource")]
    [Serializable]
    public class Resource : ScriptableObject
    {
        [SerializeField] protected ResourceType resourceType;
        [SerializeField] protected ResourceChangedEvent resourceChangedEvent;
        protected int amount;
    
        public virtual int Amount
        {
            get => amount;
            set
            {
                amount = value;
                resourceChangedEvent?.RaiseEvent(this);
            }
        }

        public ResourceType ResourceType => resourceType;

        public bool CheckAmount(int requiredAmount) => Amount >= requiredAmount;

        public virtual void Add(int amount) => Amount += amount;

        public virtual void Subtract(int amount) => Amount -= amount;
    }
}
