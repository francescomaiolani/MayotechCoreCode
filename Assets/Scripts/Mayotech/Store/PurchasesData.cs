using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.Store
{
    [CreateAssetMenu(fileName = "PurchasesData", menuName = "Store/PurchasesData")]
    [Serializable]
    public class PurchasesData : ScriptableObject
    {
        [ShowInInspector] private Dictionary<string, VirtualPurchaseDefinition> virtualPurchases = new();
        [ShowInInspector] private Dictionary<string, RealMoneyPurchaseDefinition> realMoneyPurchases = new();

        public Dictionary<string, VirtualPurchaseDefinition> VirtualPurchases
        {
            get => virtualPurchases;
            set => virtualPurchases = value;
        }

        public Dictionary<string, RealMoneyPurchaseDefinition> RealMoneyPurchases
        {
            get => realMoneyPurchases;
            set => realMoneyPurchases = value;
        }

        public VirtualPurchaseDefinition GetVirtualPuchaseDefinition(string purchaseId) =>
            VirtualPurchases.TryGetValue(purchaseId, out var purchaseDefinition) ? purchaseDefinition : null;
        
        public RealMoneyPurchaseDefinition GetRealMoneyPuchaseDefinition(string purchaseId) =>
            RealMoneyPurchases.TryGetValue(purchaseId, out var purchaseDefinition) ? purchaseDefinition : null;
    }
}