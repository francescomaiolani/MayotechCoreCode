using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Services.Economy;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.UGSResources
{
    [CreateAssetMenu(fileName = "InventoryManager", menuName = "Manager/InventoryManager")]
    public sealed class InventoryManager : Service
    {
        protected IEconomyConfigurationApiClient EconomyClient => EconomyService.Instance.Configuration;
        protected IEconomyPlayerInventoryApiClient EconomyInventory => EconomyService.Instance.PlayerInventory;

        [SerializeField] protected List<ScriptableItem> allItems;
        protected Dictionary<string, ScriptableItem> itemsDictionary = new();

        public override void InitService() =>
            itemsDictionary = allItems.ToDictionary(item => item.InventoryId, item => item);

#region Utility Methods

        protected InventoryItemDefinition GetLocalItemDefinition(string itemId)
        {
            var scriptableItem = GetScriptableItemOfType(itemId);
            return scriptableItem?.ItemDefinition;
        }

        public ScriptableItem GetScriptableItemOfType(string itemId) =>
            itemsDictionary.TryGetValue(itemId, out var scriptableItem) ? scriptableItem : null;

        public PlayersInventoryItem GetInstanceItemOfType(string itemId, string instanceId)
        {
            var item = GetScriptableItemOfType(itemId);
            return item?.GetItemInstance(instanceId) ?? null;
        }

        /// <summary>
        /// Locally assigns the definition to the scriptable object representing the item
        /// </summary>
        protected void AssignItemDefinition(InventoryItemDefinition itemDefinition)
        {
            var scriptableItem = GetScriptableItemOfType(itemDefinition.Id);
            if (scriptableItem != null)
                scriptableItem.ItemDefinition = itemDefinition;
            else
                Debug.LogError($"ScriptableItem {itemDefinition.Id} not found");
        }

        /// <summary>
        /// Assign a new item instance to the associated scriptable
        /// </summary>
        /// <param name="inventoryItem"> the new item to assign </param>
        public void AssignItemInstance(PlayersInventoryItem inventoryItem)
        {
            var scriptableItem = GetScriptableItemOfType(inventoryItem.InventoryItemId);
            if (scriptableItem != null)
                scriptableItem.AddInventoryItemInstance(inventoryItem);
            else
                Debug.LogError($"ScriptableItem {scriptableItem.InventoryId} not found");
        }

#endregion

        /// <summary>
        /// Get all items definition from server and assign them to the sriptable object in client
        /// </summary>
        public async UniTask GetInventoryItemsDefinitions()
        {
            try
            {
                var definitions = await EconomyClient.GetInventoryItemsAsync();
                foreach (var definition in definitions)
                    AssignItemDefinition(definition);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Get a single item definition and assign it to the scriptable object of the client
        /// </summary>
        /// <param name="itemId"> id of the item to get the definition </param>
        public async UniTask GetInventoryItemDefinition(string itemId)
        {
            try
            {
                var definition = await EconomyClient.GetInventoryItemAsync(itemId);
                if (definition == null) return;
                GetScriptableItemOfType(itemId).ItemDefinition = definition;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        /// <summary>
        /// Get all instances of a player item type from server based on the local item definition
        /// </summary>
        public async UniTask GetInventoryItemsOfType(string itemId)
        {
            var scriptableItem = GetScriptableItemOfType(itemId);
            var definition = scriptableItem?.ItemDefinition;
            if (definition == null)
            {
                await GetInventoryItemDefinition(itemId);
                definition = GetLocalItemDefinition(itemId);
            }

            var result = await definition.GetAllPlayersInventoryItemsAsync();
            while (result.HasNext)
            {
                result = await definition.GetAllPlayersInventoryItemsAsync();
            }

            scriptableItem.InventoryItems = result.PlayersInventoryItems;
        }

        /// <summary>
        /// Get all player inventory items and instances and assign them to the correct scriptable objects
        /// </summary>
        /// <param name="itemsPerFetch"> items to fetch per call </param>
        public async UniTask GetPlayerInventory(int itemsPerFetch = 20)
        {
            // Optional, defaults to 20
            var options = new GetInventoryOptions { ItemsPerFetch = itemsPerFetch };

            var inventoryResult = await EconomyInventory.GetInventoryAsync(options);
            var items = inventoryResult.PlayersInventoryItems;
            while (inventoryResult.HasNext)
            {
                inventoryResult = await inventoryResult.GetNextAsync(itemsPerFetch);
                items = inventoryResult.PlayersInventoryItems;
            }

            var groupedItems = items.GroupBy(item => item.InventoryItemId)
                .Select(x => x.Select(v => v).ToList())
                .ToList();
            foreach (var itemGroup in groupedItems)
            {
                var itemId = itemGroup.FirstOrDefault()?.InventoryItemId;
                var scriptableObject = GetScriptableItemOfType(itemId);
                if (scriptableObject != null)
                    scriptableObject.InventoryItems = itemGroup;
                else
                    Debug.LogError($"ScriptableItem {itemId} not found");
            }
        }

        /// <summary>
        /// Adds a new item instance to the player inventory and assigns it back to the scriptableItem
        /// </summary>
        private async UniTask AddItemToPlayersInventory(string itemId, string instanceId, Action onSuccess,
            Action<Exception> onFail, Dictionary<string, object> instanceData)
        {
            try
            {
                var options = new AddInventoryItemOptions()
                {
                    PlayersInventoryItemId = instanceId,
                    InstanceData = instanceData
                };
                var item = await EconomyInventory.AddInventoryItemAsync(itemId, options);
                AssignItemInstance(item);
                Debug.Log($"Added {item.InventoryItemId} with ID {item.PlayersInventoryItemId} to player's inventory.");
                onSuccess?.Invoke();
            }
            catch (EconomyValidationException e)
            {
                foreach (var message in e.Details.SelectMany(errorDetail => errorDetail.Messages))
                    Debug.LogError(message);
                onFail?.Invoke(e);
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogError($"{e} - Retry after {e.RetryAfter}");
                onFail?.Invoke(e);
            }
            catch (EconomyException e)
            {
                Debug.LogError(e);
                onFail?.Invoke(e);
            }
        }

        /// <summary>
        /// Updates the instance data of a player Item and assigns it back to the scriptable Item
        /// </summary>
        private async UniTask UpdatePlayersItemInstanceData(string itemId, Dictionary<string, object> instanceData,
            Action onSuccess, Action<Exception> onFail)
        {
            try
            {
                var item = await EconomyInventory.UpdatePlayersInventoryItemAsync(itemId, instanceData);
                AssignItemInstance(item);
                Debug.Log($"Added instance data to item with ID {item.PlayersInventoryItemId}");
                onSuccess?.Invoke();
            }
            catch (EconomyValidationException e)
            {
                foreach (var message in e.Details.SelectMany(errorDetail => errorDetail.Messages))
                    Debug.LogError(message);
                onFail?.Invoke(e);
            }
            catch (EconomyRateLimitedException e)
            {
                Debug.LogError($"{e} - Retry after {e.RetryAfter}");
                onFail?.Invoke(e);
            }
            catch (EconomyException e)
            {
                Debug.LogError(e);
                onFail?.Invoke(e);
            }
        }

        /// <summary>
        /// Utility method for the editor to add a scriptableItem
        /// </summary>
        public void AddItemToList(ScriptableItem scriptableItem)
        {
            if (allItems.Contains(scriptableItem))
                Debug.LogError($"Resource {scriptableItem.InventoryId}");
            else
            {
                allItems.Add(scriptableItem);
                Debug.Log($"Currency added {scriptableItem.InventoryId}");
            }
        }
    }
}