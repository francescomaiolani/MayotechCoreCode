using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace Mayotech.SafeTime
{
    [CreateAssetMenu(fileName = "TimeManager", menuName = "Manager/TimeManager")]
    public class TimeManager : Service
    {
        [SerializeField, AutoConnect] protected Clock clock;
        [SerializeField] private GameEvent<bool> onApplicationPaused;

        public const string REFERENCE_WEBSITE = "https://www.google.it";

        public Clock Clock => clock;

        public override void InitService()
        {
            clock.UtcNow = DateTime.UtcNow;
            onApplicationPaused.Subscribe(OnApplicationPause);
        }

        private void OnDestroy() => onApplicationPaused.Unsubscribe(OnApplicationPause);

        private void OnApplicationPause(bool pause)
        {
            if (!pause)
                GetServerTime();
        }

        // Track server time by storing utc when server time is requested and using a timer to
        // track time since server time was updated.
        // To calculate approximate server time, simply add stopwatch elapsed time to recorded server time.
        public async UniTask GetServerTime()
        {
            var myHttpWebRequest = UnityWebRequest.Get(REFERENCE_WEBSITE);
            await myHttpWebRequest.SendWebRequest();

            try
            {
                var netTime = myHttpWebRequest.GetResponseHeader("DATE");
                var dt = DateTime.Parse(netTime, System.Globalization.CultureInfo.InvariantCulture).ToUniversalTime();
                Debug.Log($"NOW: {dt}");
                Clock.SetServerTime(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}