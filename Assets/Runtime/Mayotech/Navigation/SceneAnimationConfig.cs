using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/SceneAnimationConfig")]
public class SceneAnimationConfig : ScriptableObject
{
    [SerializeField] private List<AnimationEffect> enterEffects, exitEffects;

    public List<AnimationEffect> EnterEffects => enterEffects;
    public List<AnimationEffect> ExitEffects => exitEffects;
}

public enum AnimationType {Move, Fade, Scale}
