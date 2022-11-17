using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mayotech.Pooling
{
    public class PoolingSystem<T> : ScriptableObject where T : PooledObject
    {
        protected int maxObject;
        protected Dictionary<string, List<T>> poolingObjects = new();

        public void Init(int maxObject)
        {
            this.maxObject = maxObject;
        }

        public void AddObjectToPool(T pooledObject)
        {
            poolingObjects.TryGetValue(pooledObject.name, out var list);
            list ??= new List<T>();
            if (list.Count >= maxObject)
                Destroy(pooledObject.gameObject);
            else
            {
                pooledObject.ShutDown();
                list.Add(pooledObject);
            }
        }

        public T GetObject(T pooledObject, Vector3 position, Transform parent)
        {
            poolingObjects.TryGetValue(pooledObject.name, out var list);
            list ??= new List<T>();
            return list.FirstOrDefault() ?? InstantiateNewObject(pooledObject, position, parent);
        }

        private T InstantiateNewObject(T pooledObject, Vector3 position, Transform parent)
        {
            var obj = Instantiate(pooledObject, position, Quaternion.identity, parent);
            return obj;
        }
    }
}