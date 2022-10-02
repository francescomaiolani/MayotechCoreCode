using System.Collections.Generic;
using Mayotech.Animation;
using UnityEngine;

namespace Mayotech.Navigation
{
    [CreateAssetMenu(menuName = "Navigation/SceneAnimationData")]
    public class SceneAnimationData : ScriptableObject
    {
        [SerializeReference] private List<AnimationEffect> enterEffects, exitEffects;

        public List<AnimationEffect> EnterEffects => enterEffects;
        public List<AnimationEffect> ExitEffects => exitEffects;
    }

    public enum AnimationType
    {
        Move,
        Fade,
        Scale
    }
}
