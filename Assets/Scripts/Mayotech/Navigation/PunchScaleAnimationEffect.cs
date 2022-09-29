using System;
using UnityEngine;

namespace Mayotech.Animation
{
    [Serializable]
    [CreateAssetMenu(menuName = "Animation/PunchScaleAnimationEffect")]
    public class PunchScaleAnimationEffect : AnimationEffect
    {
        [SerializeField] private Vector3 amount;
        
        public Vector3 Amount => amount;
    }
}