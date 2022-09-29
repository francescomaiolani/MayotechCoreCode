using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Config/SceneResolutionsConfig")]
public class SceneResolutionsConfig : ScriptableObject
{
    [SerializeField] private ResolutionMatch defaultResolutionMatch;
    [SerializeField] private List<ResolutionMatch> resolutionList;

    public ResolutionMatch DefaultResolutionMatch => defaultResolutionMatch;
    public List<ResolutionMatch> ResolutionList => resolutionList;
}