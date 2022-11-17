using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mayotech.Resources;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class ResourcesEditorWindow : OdinEditorWindow
{
    private ResourceManager resourceManager;

    [TabGroup("Tab", "Resources")]
    [TableList(AlwaysExpanded = true, NumberOfItemsPerPage = 20, HideToolbar = true, CellPadding = 5,
        IsReadOnly = true)]
    [PropertySpace(SpaceBefore = 20)]
    [SerializeField]
    private List<LocalResource> allResources = new();

    [MenuItem("Mayotech/Resources Editor")]
    private static void OpenWindow()
    {
        GetWindow<ResourcesEditorWindow>().Show();
    }

    protected override void Initialize()
    {
        WindowPadding = Vector4.one * 15;
        position = new Rect(Vector2.zero, new Vector2(800, 600));
        name = "Resources Editor Window";
        var resourceManagerguid = AssetDatabase.FindAssets("t: ResourceManager");
        resourceManager =
            AssetDatabase.LoadAssetAtPath<ResourceManager>(AssetDatabase.GUIDToAssetPath(resourceManagerguid[0]));
        FillResources();
    }

    public void FillResources()
    {
        allResources.Clear();
        var resourcesArray = AssetDatabase.FindAssets("t: LocalResource").OrderBy(item => item);
        resourcesArray.ForEach(guid => allResources.Add(
            AssetDatabase.LoadAssetAtPath<LocalResource>(AssetDatabase.GUIDToAssetPath(guid))));
    }

    [TabGroup("Tab", "Actions", Paddingless = true)]
    [BoxGroup("Tab/Actions/Create")]
    [HorizontalGroup("Tab/Actions/Create/hor", width: 50)]
    [SerializeField]
    [PreviewField(ObjectFieldAlignment.Center, Height = 40), HideLabel]
    private Sprite newResourceSprite;

    [TabGroup("Tab", "Actions", Paddingless = true)]
    [VerticalGroup("Tab/Actions/Create/hor/stat"), LabelWidth(200), LabelText("Resource Type")]
    [SerializeField]
    private string newResourceName;

    [TabGroup("Tab", "Actions", Paddingless = true)]
    [VerticalGroup("Tab/Actions/Create/hor/stat"), LabelWidth(200), LabelText("Resource Amount")]
    [SerializeField]
    private int newResourceAmount;

    [TabGroup("Tab", "Actions", Paddingless = true)]
    [VerticalGroup("Tab/Actions/Create/hor/actions")]
    [Button("Create Resource", ButtonSizes.Large, ButtonHeight = 40), GUIColor(0f, 0.8f, 0f, 1f)]
    public void CreateNewResource()
    {
        if (CheckResourceCorrectness())
        {
            var localResource = CreateInstance<LocalResource>()
                .Fill(newResourceSprite, newResourceName, newResourceAmount);
            AssetDatabase.CreateAsset(localResource, $"Assets/ScriptableObjects/LocalResource/{newResourceName}.asset");
            AssetDatabase.SaveAssets();
            localResource.AddToResourceManager();
            FillResources();
            Repaint();
        }
        else
            Debug.LogError("Resource input are invalid");
    }

    private bool CheckResourceCorrectness()
    {
        return newResourceSprite != null && newResourceName.Contains("Resource") && newResourceName.Length > 0;
    }

    [TabGroup("Tab", "Actions", Paddingless = true)]
    [PropertySpace(SpaceBefore = 15)]
    [Button("Add all resources to Manager", ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.8f, 1f)]
    public void AddAllResourcesToManager()
    {
        allResources.ForEach(resource => resourceManager.AddResourceToList(resource));
        AssetDatabase.SaveAssets();
    }
}