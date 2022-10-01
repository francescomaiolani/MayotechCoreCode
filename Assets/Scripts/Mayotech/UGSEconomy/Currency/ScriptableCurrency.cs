using System;
using Mayotech.Resources;
using Sirenix.OdinInspector;
using Unity.Services.Economy.Model;
using UnityEditor;
using UnityEngine;

namespace Mayotech.UGSResources
{
    [Serializable]
    [CreateAssetMenu(fileName = "ScriptableCurrency", menuName = "UGSResource/ScriptableCurrency")]
    public class ScriptableCurrency : ScriptableObject, IResource
    {
        [SerializeField] private StringVariable currencyId;
        [SerializeField, AutoConnect] private OnCurrencyServerBalanceChangedGameEvent onCurrencyServerChangedGameEvent;
        [SerializeField, AutoConnect] private OnCurrencyLocalBalanceChangedGameEvent onCurrencyLocalChangedGameEvent;

        private CurrencyDefinition currencyDefinition;
        private PlayerBalance currencyBalance;

        public string CurrencyId => currencyId.Value;
        public string ResourceId => currencyBalance.CurrencyId;
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
                    onCurrencyLocalChangedGameEvent?.RaiseEvent(this, newAmount - oldAmount);
            }
        }

        public void Init() => onCurrencyServerChangedGameEvent.Subscribe(OnCurrencyBalanceChanged);

        private void OnCurrencyBalanceChanged(PlayerBalance balance) => CurrencyBalance = balance;

        [Button("Add Currency to Manager", ButtonSizes.Medium)]
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