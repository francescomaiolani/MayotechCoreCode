using System;
using Mayotech.Navigation;
using UnityEngine;

namespace Mayotech.Animation
{
    public interface IAnimationEffect
    {
        AnimationType AnimationType { get; }
        float Duration { get; }
        float Delay { get; }
        AnimationCurve Curve { get; }
    }
    
    [Serializable]
    public abstract class AnimationEffect : ScriptableObject, IAnimationEffect
    {
        [SerializeField] protected AnimationType animationType;
        [SerializeField] protected float duration, delay;
        [SerializeField] protected AnimationCurve curve;
        
        public AnimationType AnimationType => animationType;
        public float Duration => duration;
        public float Delay => delay;
        public AnimationCurve Curve => curve;
       
    }
}
