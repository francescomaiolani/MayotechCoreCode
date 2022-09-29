using System;
using UnityEngine;

namespace Mayotech.Animation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Animation/MoveAnimationEffect")]
    public class MoveAnimationEffect : AnimationEffect
    {
        [SerializeField] private Vector3 from, to;
        
        public Vector3 From => from;
        public Vector3 To => to;
    }
}