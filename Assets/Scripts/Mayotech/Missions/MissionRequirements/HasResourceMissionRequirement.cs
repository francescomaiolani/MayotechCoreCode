using Mayotech.Resources;
using UnityEngine;

namespace Mayotech.Missions
{
    [CreateAssetMenu(menuName = "Mission/MissionRequirement/HasResourceMissionRequirement")]
    public class HasResourceMissionRequirement : MissionRequirement
    {
        protected ResourceManager ResourceManager => ServiceLocator.Instance.ResourceManager;

        [SerializeField] protected ResourceType resourceType;
        [SerializeField] protected int resourceToPossess;

        public override void CheckRequirement()
        {
            if (!(ResourceManager.GetResource(resourceType)?.Amount >= resourceToPossess)) return;
            
            Completed = true;
        }
    }
}