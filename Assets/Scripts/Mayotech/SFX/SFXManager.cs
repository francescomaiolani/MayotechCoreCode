using System;
using Cysharp.Threading.Tasks;
using Mayotech.Pooling;
using UnityEngine;

namespace Mayotech.SFX
{
    [CreateAssetMenu(menuName = "Manager/SFXManager", fileName = "SFXManager")]
    public class SFXManager : Service
    {
        [SerializeField] protected SFXPoolingSystem pooledSFX;

        public override void InitService()
        {
            pooledSFX.Init(3);
        }

        public async UniTask SpawnFX(SFX prefab, Vector3 position, Transform parent, float fxDuration,
            Action onFxCompleted = null)
        {
            var fx = pooledSFX.GetObject(prefab, position, parent);
            fx.gameObject.SetActive(true);
            fx.Initialize(position, parent);
            await UniTask.Delay((int)(fxDuration * 1000), DelayType.DeltaTime);
            pooledSFX.AddObjectToPool(fx);
            onFxCompleted?.Invoke();
        }
    }
}