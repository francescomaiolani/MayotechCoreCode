using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(fileName = "LocalResource", menuName = "Resource/LocalResource")]
    [Serializable]
    public class LocalResource : ScriptableObject, IResource
    {
        [TableColumnWidth(50, resizable: false)]
        [PreviewField(ObjectFieldAlignment.Center, Height = 40), HideLabel]
        [SerializeField]
        [VerticalGroup("Sprite")]
        protected Sprite resourceSprite;

        [VerticalGroup("Resource"), LabelWidth(200)] [SerializeField] protected string resourceType;
        [VerticalGroup("Resource"), LabelWidth(200)] [SerializeField] protected long amount;
        [SerializeField, AutoConnect, HideInInspector] protected OnResourceChangedEvent resourceChangedEvent;

        public virtual long Amount
        {
            get => amount;
            set
            {
                var oldAmount = amount;
                amount = value;
                resourceChangedEvent?.RaiseEvent(this, value - oldAmount);
            }
        }

        public string ResourceType => resourceType;
        public string ResourceId => resourceType;

        public LocalResource Fill(Sprite sprite, string resourceType, int resourceAmount)
        {
            resourceSprite = sprite;
            this.resourceType = resourceType;
            Amount = resourceAmount;
            return this;
        }

        public bool CheckAmount(int requiredAmount) => Amount >= requiredAmount;

        public virtual void Add(int amount) => Amount += amount;

        public virtual void Subtract(int amount) => Amount -= amount;

        public void AddToResourceManager()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: ResourceManager");
            var manager = AssetDatabase.LoadAssetAtPath<ResourceManager>(AssetDatabase.GUIDToAssetPath(guids[0]));
            manager.AddResourceToList(this);
#endif
        }

        [Button("X", ButtonSizes.Small), GUIColor(1f, 0.6f, 0.4f, 1f)]
        [TableColumnWidth(50, resizable: false)]
        [VerticalGroup("Actions")]
        public void DeleteResource()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: ResourceManager");
            var manager = AssetDatabase.LoadAssetAtPath<ResourceManager>(AssetDatabase.GUIDToAssetPath(guids[0]));
            manager.RemoveResourceFromList(this);
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(this));
#endif
        }

        [Button("✓", ButtonSizes.Small), GUIColor(0f, 0.8f, 0f, 1f)]
        [TableColumnWidth(50, resizable: false)]
        [VerticalGroup("Actions")]
        public void SaveResource()
        {
#if UNITY_EDITOR
            var path = AssetDatabase.GetAssetPath(this);
            var asset = AssetDatabase.LoadAssetAtPath<LocalResource>(path);
            AssetDatabase.RenameAsset(path, $"{resourceType}");
            AssetDatabase.SaveAssets();
#endif
        }
    }
}