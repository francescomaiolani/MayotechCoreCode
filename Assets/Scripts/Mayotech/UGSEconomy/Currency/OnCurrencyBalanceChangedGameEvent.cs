using UnityEngine;

namespace Mayotech.UGSEconomy.Currency
{
    [CreateAssetMenu(fileName = "OnCurrencyBalanceChanged", menuName = "GameEvent/OnCurrencyBalanceChangedGameEvent")]
    public class OnCurrencyBalanceChangedGameEvent : GameEvent<ScriptableCurrency, long> { }
}