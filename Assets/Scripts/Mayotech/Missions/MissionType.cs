using System;
using UnityEngine;

namespace Mayotech.Missions
{
    [Serializable]
    [CreateAssetMenu(menuName = "Mission/MissionType")]
    public class MissionType : ScriptableObject
    {
        [SerializeField] protected string missionType;

        public string MissionType1 => missionType;
    }
}