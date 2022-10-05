using System;
using System.Diagnostics;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Mayotech.SafeTime
{
    [CreateAssetMenu(fileName = "Clock", menuName = "Clock")]
    public class Clock : ScriptableObject
    {
        public DateTime UtcNow { get; set; }
        protected Stopwatch serverTimeStopwatch = new(); 
        
        public DateTime CurrentUtcTime => UtcNow + serverTimeStopwatch.Elapsed;
        
        public void SetServerTime(DateTime serverDateTime)
        {
            UtcNow = serverDateTime;
            // Start the timer so we can always calculate the server time by adding elapsed to start time.
            serverTimeStopwatch.Restart();
        }
    }
}