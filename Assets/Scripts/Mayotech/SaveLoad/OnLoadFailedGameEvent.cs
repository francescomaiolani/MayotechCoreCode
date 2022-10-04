using System;
using UnityEngine;

namespace Mayotech.SaveLoad
{
    [CreateAssetMenu(menuName = "GameEvent/OnLoadFailedGameEvent")]
    public class OnLoadFailedGameEvent : GameEvent<Exception> { }
}