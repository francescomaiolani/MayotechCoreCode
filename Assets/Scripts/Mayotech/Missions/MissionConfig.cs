using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mayotech.Missions
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mission/MissionConfig")]
    public class MissionConfig : ScriptableObject
    {
        [SerializeField] protected string missionName;
        [SerializeField] protected string missionDescription;

        [SerializeField] protected List<MissionRequirement> missionRequirements;
        [SerializeField] protected List<MissionReward> missionRewards;
        [SerializeField, AutoConnect] protected OnMissionCompletedGameEvent onMissionCompleted;

        public void InitMission()
        {
            missionRequirements.ForEach(item => item.Init(OnRequirementSatisfied));
        }

        private void OnRequirementSatisfied(MissionRequirement missionRequirement)
        {
            if (!missionRequirements.Contains(missionRequirement)) return;
            
            var missionCompleted = CheckMission();
            if (missionCompleted) 
                onMissionCompleted?.RaiseEvent(this);
        }

        private bool CheckMission() => missionRequirements.All(item => item.Completed);
        
    }
}