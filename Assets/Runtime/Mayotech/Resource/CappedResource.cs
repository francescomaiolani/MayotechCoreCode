using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Config/CappedResource")]
    public class CappedResource : Resource
    {
        protected bool hasMinCap, hasMaxCap;
        protected int minCap, maxCap;
        [SerializeField] protected ResourceMaxCapEvent resourceMaxCapEvent;
        [SerializeField] protected ResourceMinCapEvent resourceMinCapEvent;

        public override int Amount
        {
            get => amount;
            set
            {
                if (hasMinCap && value < minCap)
                {
                    amount = minCap;
                    resourceMinCapEvent?.RaiseEvent(this);
                }
                else if (hasMaxCap && value > maxCap)
                {
                    amount = maxCap;
                    resourceMaxCapEvent?.RaiseEvent(this);
                }
                else
                {
                    amount = value;
                }
                resourceChangedEvent?.RaiseEvent(this);
            }
        }
    }
}
