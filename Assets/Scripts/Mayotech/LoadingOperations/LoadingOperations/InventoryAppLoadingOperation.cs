using System;
using Mayotech.UGSEconomy.Inventory;
using UnityEngine;

namespace Mayotech.AppLoading
{
    public class InventoryAppLoadingOperation : AppLoadingOperation
    {
        private InventoryManager InventoryManager => ServiceLocator.Instance.InventoryManager;
        
        public override async void StartOperation()
        {
            base.StartOperation();
            try
            {
                await InventoryManager.GetRemoteItemsDefinitions();
                await InventoryManager.GetRemoteInventory();  
                Status = LoadingOperationStatus.Completed;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                Status = LoadingOperationStatus.Failed;
            }
        }
    }
}
