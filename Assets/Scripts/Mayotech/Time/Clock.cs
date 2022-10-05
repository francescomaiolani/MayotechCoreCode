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
        protected DateTime CurrentUtcTime => UtcNow + serverTimeStopwatch.Elapsed;

        protected readonly DateTime startEpochTime = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public void SetServerTime(DateTime serverDateTime)
        {
            UtcNow = serverDateTime;
            // Start the timer so we can always calculate the server time by adding elapsed to start time.
            serverTimeStopwatch.Restart();
        }
    }
}