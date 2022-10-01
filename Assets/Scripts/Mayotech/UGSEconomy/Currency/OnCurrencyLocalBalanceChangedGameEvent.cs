using UnityEngine;

namespace Mayotech.UGSResources
{
    [CreateAssetMenu(fileName = "OnCurrencyLocalBalanceChanged", menuName = "GameEvent/OnCurrencyLocalBalanceChanged")]
    public class OnCurrencyLocalBalanceChangedGameEvent : GameEvent<ScriptableCurrency, long> { }
}