using System.Collections.Generic;
using UnityEngine;

namespace Mayotech.SaveLoad
{
    [CreateAssetMenu(menuName = "GameEvent/OnLoadCompletedGameEvent")]
    public class OnLoadCompletedGameEvent : GameEvent<Dictionary<string, string>> { }
}