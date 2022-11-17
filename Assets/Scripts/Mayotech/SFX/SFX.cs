using System.Linq;
using Mayotech.Pooling;
using Sirenix.Utilities;
using UnityEngine;

namespace Mayotech.SFX
{
    public class SFX : PooledObject
    {
        [SerializeField] private ParticleSystem[] particleSystems;

        public int LifetimeMs => (int)(particleSystems.Max(item => item.main.duration) * 1000);

        public override void Initialize(Vector3 position, Transform parent)
        {
            particleSystems.ForEach(item =>
            {
                item.Clear(true);
                item.Play(true);
            });
        }

        public override void ShutDown()
        {
            gameObject.SetActive(false);
        }
    }
}