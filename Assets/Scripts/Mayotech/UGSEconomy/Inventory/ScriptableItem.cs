using System;
using System.Collections.Generic;
using System.Linq;
using Mayotech.Resources;
using Sirenix.OdinInspector;
using Unity.Services.Economy.Model;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSResources
{
    [Serializable]
    [CreateAssetMenu(fileName = "ScriptableItem", menuName = "UGSResource/ScriptableItem")]
    public class ScriptableItem : ScriptableObject, IResource
    {
        [SerializeField] protected StringVariable inventoryId;
        [SerializeField, AutoConnect] protected OnItemChangedGameEvent onItemChanged;

        protected InventoryItemDefinition itemDefinition;
        protected List<PlayersInventoryItem> inventoryItems = new();

        public string ResourceId => itemDefinition.Id;
        public string InventoryId => inventoryId.Value;

        public InventoryItemDefinition ItemDefinition
        {
            get => itemDefinition;
            set => itemDefinition = value;
        }

        public List<PlayersInventoryItem> InventoryItems
        {
            get => inventoryItems;
            set
            {
                inventoryItems = value;
                onItemChanged?.RaiseEvent(inventoryItems);
            }
        }

        public PlayersInventoryItem GetItemInstance(string instanceId) =>
            InventoryItems.FirstOrDefault(item => item.PlayersInventoryItemId == instanceId);

        public void AddInventoryItemInstance(PlayersInventoryItem itemInstance)
        {
            var index = InventoryItems.FindIndex(item =>
                item.PlayersInventoryItemId == itemInstance.PlayersInventoryItemId);
            if (index > -1)
                InventoryItems[index] = itemInstance;
            else
                InventoryItems.Add(itemInstance);
            onItemChanged?.RaiseEvent(new []{itemInstance});
        }

        [Button("Add Item to Manager", ButtonSizes.Medium)]
        public void AddItemToConfig()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: InventoryManager");
            var manager = AssetDatabase.LoadAssetAtPath<InventoryManager>(AssetDatabase.GUIDToAssetPath(guids[0]));
            manager.AddItemToList(this);
#endif
        }
    }
}