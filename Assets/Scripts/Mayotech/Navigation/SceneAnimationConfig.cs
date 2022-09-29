using System.Collections.Generic;
using Mayotech.Animation;
using UnityEngine;

namespace Mayotech.Navigation
{
    [CreateAssetMenu(menuName = "Config/SceneAnimationConfig")]
    public class SceneAnimationConfig : ScriptableObject
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
