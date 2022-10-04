using Mayotech.Resources;
using UnityEngine;

namespace Mayotech.Missions
{
    [CreateAssetMenu(menuName = "Mission/MissionRequirement/HasResourceMissionRequirement")]
    public class HasResourceMissionRequirement : MissionRequirement
    {
        [SerializeField] protected LocalResource resource;
        [SerializeField] protected int resourceToPossess;

        public override void CheckRequirement()
        {
            if (!(resource.Amount >= resourceToPossess)) return;
            
            Completed = true;
        }
    }
}