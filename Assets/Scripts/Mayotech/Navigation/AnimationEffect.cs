using System;
using UnityEngine;

[Serializable]
public class AnimationEffect
{
    [SerializeField] private AnimationType animationType;
    [SerializeField] private float duration, delay;
    [SerializeField] private AnimationCurve curve;

    public AnimationType AnimationType => animationType;
    public float Duration => duration;
    public float Delay => delay;
    public AnimationCurve Curve => curve;
}