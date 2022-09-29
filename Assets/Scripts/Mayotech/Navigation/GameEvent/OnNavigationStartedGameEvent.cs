using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mayotech.Navigation
{
    [CreateAssetMenu(menuName = "GameEvent/OnNavigationStartedGameEvent")]
    public class OnNavigationStartedGameEvent : GameEvent<string, string> { }
}