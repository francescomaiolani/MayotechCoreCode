using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mayotech.Navigation
{
    [CreateAssetMenu(menuName = "GameEvent/OnNavigationEndedGameEvent")]
    public class OnNavigationEndedGameEvent : GameEvent<string, string> { }
}