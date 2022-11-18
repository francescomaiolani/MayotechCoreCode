using UnityEngine;

namespace Mayotech.Missions
{
    [CreateAssetMenu(menuName = "Manager/MissionManager")]
    public class MissionManager : Service
    {
        public override void InitService() { }

        public override bool CheckServiceIntegrity() => true;
    }
}