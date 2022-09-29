using System;
using UnityEngine;

namespace Mayotech.Animation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Animation/FadeAnimationEffect")]
    public class FadeAnimationEffect : AnimationEffect
    {
        [SerializeField] private float from, to;
        
        public float From => from;
        public float To => to;
    }
}