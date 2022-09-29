using System;
using Mayotech.Resources;
using UnityEngine;

namespace Mayotech.Missions
{
    [CreateAssetMenu(menuName = "Mission/MissionRequirement/CollectResourceMissionRequirement")]
    public class CollectResourceMissionRequirement : MissionRequirement
    {
        protected ResourceManager ResourceManager => ServiceLocator.Instance.ResourceManager;
        [SerializeField] protected OnResourceChangedEvent listenedEvent;

        [SerializeField] protected ResourceType resourceType;
        [SerializeField] protected int maxResourceToCollect;
        
        protected int resourceCollected;

        public override void Init(Action<MissionRequirement> onRequirementSatisfied)
        {
            listenedEvent.Subscribe(OnEventListened);
            resourceCollected = 0;
            base.Init(onRequirementSatisfied);
        }

        private void OnEventListened(Resource resource, int delta)
        {
            if (resource.ResourceType != resourceType) return;
            if (delta <= 0) return;
            resourceCollected += delta;
            CheckRequirement();
        }
        
        public override void CheckRequirement()
        {
            if (resourceCollected < maxResourceToCollect) return;
            Completed = true;
        }
    }
}