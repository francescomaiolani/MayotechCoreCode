using System;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntConfig", menuName = "Config/IntConfig")]
    public class IntConfig : Config<int> { }
}