using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasScaler))]
[ExecuteAlways]
public class VerticalResolutionMatcherConfig : MonoBehaviour
{
    [SerializeField] private CanvasScaler _canvasScaler;
    private CanvasScaler CanvasScaler => _canvasScaler ? _canvasScaler : GetComponent<CanvasScaler>();

    [SerializeField] private SceneResolutionsConfig sceneResolutionsConfig;

    //solo in editor!! non usare
    private int previousScreenWidth, previousScreenHeight;

    private void OnEnable() => ApplyMatching();

    private void Awake()
    {
        previousScreenHeight = Screen.height;
        previousScreenWidth = Screen.width;
        ApplyMatching();
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    private void Update()
    {
        if (Screen.width == previousScreenWidth && Screen.height == previousScreenHeight) return;

        ApplyMatching();
        previousScreenHeight = Screen.height;
        previousScreenWidth = Screen.width;
    }
#endif

    private void ApplyMatching()
    {
        var currentResolution = sceneResolutionsConfig.DefaultResolutionMatch;

        //variabili di supporto per i calcoli
        var screenSize = Application.isEditor
            ? new Resolution { width = Screen.width, height = Screen.height }
            : Screen.currentResolution;
        
        var currentRatio = (float)screenSize.width / (float)screenSize.height;
        var aspectRatioDistance = Mathf.Abs(currentResolution.AspectRatio - currentRatio);
        var distance = aspectRatioDistance;

        foreach (var res in sceneResolutionsConfig.ResolutionList)
        {
            distance = Mathf.Abs(currentRatio - res.AspectRatio);
            if (!(distance < aspectRatioDistance)) continue;
            currentResolution = res;
            aspectRatioDistance = distance;
        }

        CanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        CanvasScaler.matchWidthOrHeight = currentResolution.Matching;
    }
}