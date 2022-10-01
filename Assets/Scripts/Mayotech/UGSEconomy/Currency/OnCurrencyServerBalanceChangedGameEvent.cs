using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.UGSResources
{   
    [CreateAssetMenu(fileName = "OnCurrencyServerBalanceChanged", menuName = "GameEvent/OnCurrencyServerBalanceChanged")]
    public class OnCurrencyServerBalanceChangedGameEvent : GameEvent<PlayerBalance> { }
}