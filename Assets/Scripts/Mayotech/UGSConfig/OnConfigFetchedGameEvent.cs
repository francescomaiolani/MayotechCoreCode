using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [CreateAssetMenu(fileName = "OnConfigFetched", menuName = "GameEvent/OnConfigFetchedGameEvent")]
    public class OnConfigFetchedGameEvent : GameEvent<JToken> { }
}