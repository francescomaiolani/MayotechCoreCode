using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "Resource/CappedResource")]
    public class CappedLocalResource : LocalResource
    {
        protected bool hasMinCap, hasMaxCap;
        protected int minCap, maxCap;
        [SerializeField] protected OnResourceMaxCapEvent onResourceMaxCapEvent;
        [SerializeField] protected OnResourceMinCapEvent onResourceMinCapEvent;

        public override long Amount
        {
            get => amount;
            set
            {
                var oldAmout = amount;
                if (hasMinCap && value < minCap)
                {
                    amount = minCap;
                    onResourceMinCapEvent?.RaiseEvent(this);
                }
                else if (hasMaxCap && value > maxCap)
                {
                    amount = maxCap;
                    onResourceMaxCapEvent?.RaiseEvent(this);
                }
                else
                {
                    amount = value;
                }
                resourceChangedEvent?.RaiseEvent(this, amount - oldAmout);
            }
        }
    }
}
