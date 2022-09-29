using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Mayotech.Animation;
using UnityEngine;

namespace Mayotech.Navigation
{
    public interface ISceneController
    { 
        void ApplySceneContext(ISceneContext sceneContext);
    }

    [Serializable]
    public abstract class SceneController : MonoBehaviour, ISceneController
    {
        [SerializeField] protected SceneAnimationConfig sceneAnimationConfig;
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected Transform content;

        public abstract void ApplySceneContext(ISceneContext sceneContext);
        
        public abstract void OnSceneLoaded();

        public async UniTask NavigateAway() => StartSceneAnimations(sceneAnimationConfig.ExitEffects);
        
        public async UniTask NavigateHere() => StartSceneAnimations(sceneAnimationConfig.EnterEffects);
        
        protected UniTask StartSceneAnimations(List<AnimationEffect> effects)
        {
            foreach (var effect in effects)
            {
                switch (effect.AnimationType)
                {
                    case AnimationType.Move:
                        Move(effect);
                        break;
                    case AnimationType.Fade:
                        Fade(effect);
                        break;
                    case AnimationType.Scale:
                        Scale(effect);
                        break;
                }
            }
            return UniTask.Delay(GetAnimationDuration(effects));
        }
        
        protected int GetAnimationDuration(List<AnimationEffect> effects)
        {
            var maxDuration = Mathf.Max(effects.Select(item => item.Duration).ToArray());
            return effects.Count == 0 ? 0 : (int)(maxDuration * 1000f);
        }
        
        protected void Move(AnimationEffect animationEffect)
        {
            if (content == null) return;
            var effect = animationEffect as MoveAnimationEffect;
            content.DOKill();
            content.position = effect.From;
            content?.DOMove(effect.To, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
        }
        
        protected void Scale(AnimationEffect animationEffect)
        {
            if (content == null) return;
            var effect = animationEffect as ScaleAnimationEffect;
            content.DOKill();
            content.localScale = effect.From;
            content?.DOScale(effect.To, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
        }
        
        protected void Fade(AnimationEffect animationEffect)
        {
            if (canvasGroup == null) return;
            var effect = animationEffect as FadeAnimationEffect;
            canvasGroup.DOKill();
            canvasGroup.alpha = effect.From;
            canvasGroup?.DOFade(effect.To, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
        }
    }
}