using System;
using Mayotech.Resources;
using Sirenix.OdinInspector;
using Unity.Services.Economy.Model;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSEconomy.Currency
{
    [Serializable]
    [CreateAssetMenu(fileName = "ScriptableCurrency", menuName = "UGSResource/ScriptableCurrency")]
    public class ScriptableCurrency : ScriptableObject, IResource
    {
        [SerializeField] private string currencyId;
        [SerializeField, AutoConnect] private OnCurrencyBalanceChangedGameEvent onCurrencyChangedGameEvent;

        private CurrencyDefinition currencyDefinition;
        [ShowInInspector] private PlayerBalance currencyBalance;

        public string CurrencyId => currencyId;
        public long Amount => currencyBalance.Balance;
        
        public CurrencyDefinition CurrencyDefinition
        {
            get => currencyDefinition;
            set => currencyDefinition = value;
        }

        public PlayerBalance CurrencyBalance
        {
            get => currencyBalance;
            set
            {
                var oldAmount = currencyBalance?.Balance ?? 0;
                currencyBalance = value;
                var newAmount = currencyBalance?.Balance ?? 0;
                if (oldAmount != newAmount)
                    onCurrencyChangedGameEvent?.RaiseEvent(this, newAmount - oldAmount);
            }
        }

        public void UpdateCurrencyAmount(long amount)
        {
            if (currencyBalance == null) return;
            
            currencyBalance.Balance += amount;
            if (amount != 0)
                onCurrencyChangedGameEvent?.RaiseEvent(this, amount);
        }
        
        [Button("Add Currency to Manager", ButtonSizes.Large),GUIColor(0.3f, 0.8f, 0.8f, 1f)]
        public void AddCurrencyToConfig()
        {
#if UNITY_EDITOR
            var guids = AssetDatabase.FindAssets("t: CurrencyManager");
            var manager = AssetDatabase.LoadAssetAtPath<CurrencyManager>(AssetDatabase.GUIDToAssetPath(guids[0]));
            manager.AddCurrencyToList(this);
#endif
        }
    }
}