using System;
using UnityEngine;

namespace Mayotech.Missions
{
    [Serializable]
    public abstract class MissionRequirement : ScriptableObject
    {
        [SerializeField] protected MissionType missionType;

        public bool Completed
        {
            get => completed;
            protected set
            {
                completed = value;
                if (value)
                    onRequirementSatisfied?.Invoke(this);
            }
        }

        protected Action<MissionRequirement> onRequirementSatisfied;
        [SerializeField] private bool completed;

        public virtual void Init(Action<MissionRequirement> onRequirementSatisfied)
        {
            Completed = false;
            this.onRequirementSatisfied = onRequirementSatisfied;
            CheckRequirement();
        }
        
        public abstract void CheckRequirement();
    }
}