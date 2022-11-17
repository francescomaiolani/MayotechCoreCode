using UnityEngine;

namespace Mayotech.Pooling
{
    public abstract class PooledObject : MonoBehaviour
    {
        public abstract void Initialize(Vector3 position, Transform parent);
        public abstract void ShutDown();
    }
}