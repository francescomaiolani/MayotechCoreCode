using UnityEngine;

namespace Mayotech.Resources
{
    [CreateAssetMenu(menuName = "GameEvent/OnResourceChangedEvent")]
    public class OnResourceChangedEvent : GameEvent<LocalResource, long>{}
}
