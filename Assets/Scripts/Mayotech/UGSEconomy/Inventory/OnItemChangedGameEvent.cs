using System.Collections.Generic;
using Unity.Services.Economy.Model;
using UnityEngine;

namespace Mayotech.UGSResources
{
    [CreateAssetMenu(fileName = "OnItemChangedGameEvent", menuName = "GameEvent/OnItemChangedGameEvent")]
    public class OnItemChangedGameEvent : GameEvent<IEnumerable<PlayersInventoryItem>> { }
}