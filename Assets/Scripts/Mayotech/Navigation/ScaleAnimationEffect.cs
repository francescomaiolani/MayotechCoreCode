using System;
using UnityEngine;

namespace Mayotech.Animation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Animation/ScaleAnimationEffect")]
    public class ScaleAnimationEffect : AnimationEffect
    {
        [SerializeField] private Vector3 from, to;
        
        public Vector3 From => from;
        public Vector3 To => to;
    }
}