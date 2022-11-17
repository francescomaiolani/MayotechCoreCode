using System.Collections;
using System.Collections.Generic;
using Mayotech.Navigation;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using WhatWapp.SceneDirector;

public class CreateSceneEditorWindow : OdinEditorWindow
{
    [MenuItem("Mayotech/CreateNewScene")]
    private static void OpenWindow()
    {
        GetWindow<CreateSceneEditorWindow>().Show();
    }

    protected override void Initialize()
    {
        WindowPadding = Vector4.one * 15;
        position = new Rect(new Vector2(Screen.width / 2, Screen.height / 2), new Vector2(400, 300));
        name = "Create Scene Window";
    }

    [ShowInInspector] private readonly string folderScenePath = "Assets/Scenes/";

    [Header("Scene Data")] [Space] [SerializeField] private string sceneName;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private bool keepLoaded;
    [SerializeField] private bool saveInSceneHistory;
    [Space] [SerializeField] private bool addDefaultObjects;

    [Button("Create Scene", ButtonSizes.Large)]
    public void CreateScene()
    {
        var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        RenderSettings.skybox = null;
        if (addDefaultObjects)
        {
            var canvasGo = new GameObject("Canvas",
                typeof(Canvas),
                typeof(CanvasScaler),
                typeof(VerticalResolutionMatcherConfig),
                typeof(AttachMainCameraToCanvas));
            var canvas = canvasGo.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            var canvasScaler = canvasGo.GetComponent<CanvasScaler>();
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            var resolutionMatcher = canvasGo.GetComponent<VerticalResolutionMatcherConfig>();
            resolutionMatcher.CanvasScaler = canvasScaler;
            resolutionMatcher.enabled = true;
        }

        var sceneData = CreateInstance<SceneData>()
            .Fill(sceneName, sceneType, keepLoaded, saveInSceneHistory);
        AssetDatabase.CreateAsset(sceneData, $"Assets/ScriptableObjects/SceneData/{sceneName}.asset");
        AssetDatabase.SaveAssets();
        sceneData.AddSceneToNavigationManager();
        EditorSceneManager.SaveScene(scene, $"{folderScenePath}{sceneName}.unity", false);
    }
}