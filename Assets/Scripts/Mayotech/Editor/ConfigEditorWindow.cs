using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mayotech.UGSConfig;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class ConfigEditorWindow : OdinEditorWindow
{
    [TableList(AlwaysExpanded = true, ShowIndexLabels = false, IsReadOnly = true, CellPadding = 5,
        DrawScrollView = false)]
    [SerializeField]
    protected readonly List<Config> allConfigs = new();

    [MenuItem("Mayotech/Config Editor")]
    private static void OpenEditor() => GetWindow<ConfigEditorWindow>();

    protected override void Initialize()
    {
        allConfigs.Clear();
        WindowPadding = Vector4.one * 15;
        position = new Rect(Vector2.zero, new Vector2(800, 600));
        name = "Config Editor Window";
        var configGuids = AssetDatabase.FindAssets("t: Config").OrderBy(item => item);
        configGuids.ForEach(guid =>
            allConfigs.Add(AssetDatabase.LoadAssetAtPath<Config>(AssetDatabase.GUIDToAssetPath(guid))));
        Repaint();
    }
}