using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "SceneConfig")]
public class SceneConfig : ScriptableObject
{
    [SerializeField] private string sceneName;
    [SerializeField] private SceneType sceneType;
    [SerializeField] private bool keepLoaded;
    [SerializeField] private bool saveInSceneHistory;

    public string SceneName => sceneName;
    public SceneType SceneType => sceneType;
    public bool KeepLoaded => keepLoaded;
    public bool SaveInSceneHistory => saveInSceneHistory;

    private void OnValidate()
    {
        if (keepLoaded && !saveInSceneHistory)
        {
            keepLoaded = false;
        }
    }
}

public enum SceneType{Screen, Panel, Popup}