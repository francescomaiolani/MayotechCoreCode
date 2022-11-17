using System;
using System.Collections.Generic;
using System.Linq;
using Mayotech.Resources;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Unity.Services.Economy.Model;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSEconomy.Inventory
{
    [Serializable]
    [CreateAssetMenu(fileName = "ScriptableItem", menuName = "UGSResource/ScriptableItem")]
    public class ScriptableItem : ScriptableObject, IResource
    {
        [SerializeField] protected string itemId;
        [SerializeField, AutoConnect] protected OnItemChangedGameEvent onItemChanged;

        protected InventoryItemDefinition itemDefinition;
        [TableList] [ShowInInspector] protected List<PlayersInventoryItem> inventoryItems = new();

        public string ItemId => itemId;

        public long Amount => inventoryItems?.Count ?? 0;

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

        public void AssignItemInstance(PlayersInventoryItem inventoryItem)
        {
            if (InventoryItems.All(item => item.PlayersInventoryItemId != inventoryItem.PlayersInventoryItemId))
                InventoryItems.Add(inventoryItem);
        }

        public void AddInventoryItemInstance(string itemId, params string[] itemInstancesId)
        {
            foreach (var instanceId in itemInstancesId)
            {
                if (InventoryItems.Any(item => item.PlayersInventoryItemId == instanceId))
                    continue;
                InventoryItems.Add(new PlayersInventoryItem(itemId, instanceId));
            }

            onItemChanged?.RaiseEvent(InventoryItems.FindAll(item =>
                itemInstancesId.Contains(item.PlayersInventoryItemId)));
        }

        public void RemoveInventoryItemInstance(params string[] itemInstancesId)
        {
            InventoryItems.RemoveAll(item => itemInstancesId.Contains(item.PlayersInventoryItemId));
        }

        [Button("Add Item to Manager", ButtonSizes.Large), GUIColor(0.3f, 0.8f, 0.8f, 1f)]
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