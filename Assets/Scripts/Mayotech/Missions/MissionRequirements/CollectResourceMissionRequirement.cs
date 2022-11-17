using System;
using Mayotech.Resources;
using UnityEngine;

namespace Mayotech.Missions
{
    [CreateAssetMenu(menuName = "Mission/MissionRequirement/CollectResourceMissionRequirement")]
    public class CollectResourceMissionRequirement : MissionRequirement
    {
        protected IResourceManager ResourceManager => ServiceLocator.Instance.ResourceManager;
        [SerializeField] protected OnResourceChangedEvent listenedEvent;

        [SerializeField] protected LocalResource resource;
        [SerializeField] protected long maxResourceToCollect;

        protected long resourceCollected;

        public override void Init(Action<MissionRequirement> onRequirementSatisfied)
        {
            listenedEvent.Subscribe(OnEventListened);
            resourceCollected = 0;
            base.Init(onRequirementSatisfied);
        }

        private void OnDestroy() => listenedEvent.Unsubscribe(OnEventListened);

        private void OnEventListened(LocalResource localResource, long delta)
        {
            if (localResource != resource) return;
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