using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

[Serializable]
public class SceneController : MonoBehaviour
{
    [SerializeField] private SceneConfig sceneConfig;
    [SerializeField] private SceneAnimationConfig sceneAnimationConfig;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Transform content;
    
    private int GetAnimationDuration(List<AnimationEffect> effects) 
    {
        var maxDuration = Mathf.Max(effects.Select(item => item.Duration).ToArray());
        return (int)(maxDuration * 1000f);
    }

    public async UniTask NavigateAway()
    {
        var exitEffects = sceneAnimationConfig.ExitEffects;
        foreach (var effect in exitEffects)
        {
            switch (effect.AnimationType)
            {
                case AnimationType.Move: 
                    content.DOMove(Vector3.zero, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                case AnimationType.Fade:
                    canvasGroup.DOFade(1, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                case AnimationType.Scale:
                    content.DOScale(Vector3.one, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        await UniTask.Delay(GetAnimationDuration(exitEffects));
    }

    public async UniTask NavigateHere()
    {
        var enterEffects = sceneAnimationConfig.EnterEffects;
        foreach (var effect in enterEffects)
        {
            switch (effect.AnimationType)
            {
                case AnimationType.Move: 
                    content.DOMove(Vector3.zero, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                case AnimationType.Fade:
                    canvasGroup.DOFade(1, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                case AnimationType.Scale:
                    content.DOScale(Vector3.one, effect.Duration).SetDelay(effect.Delay).SetEase(effect.Curve);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        await UniTask.Delay(GetAnimationDuration(enterEffects));
    }

    public void OnSceneLoaded()
    {
        
    }
    
}