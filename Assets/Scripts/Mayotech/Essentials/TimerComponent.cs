using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mayotech.Timer
{
    [Serializable]
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] protected UnityEvent onTimerEnded;
        protected float timerCurrentValue;
        protected bool Ticking { get; set;}
        protected bool Repeatable { get; set;}
        protected float TimerMaxValue { get; set;}

        protected float TimerCurrentValue
        {
            get => timerCurrentValue;
            set
            {
                timerCurrentValue = value;
                if (timerCurrentValue > 0) return;
                
                onTimerEnded?.Invoke();
                if (Repeatable)
                    timerCurrentValue = TimerMaxValue;
                else
                    StopTimer();
            }
        }

        public void Init(float timerAmount, bool repeatable = false)
        {
            TimerMaxValue = timerAmount;
            Repeatable = repeatable;
            Ticking = true;
        }

        public void StopTimer()
        {
            Ticking = false;
            TimerCurrentValue = TimerMaxValue;
        }

        public void PauseTimer() => Ticking = false;
        public void ResumeTimer() => Ticking = true;

        protected void Update()
        {
            if (!Ticking) return;
            TimerCurrentValue -= Time.deltaTime;
        }
    }
}