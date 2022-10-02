using System;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoolConfig", menuName = "Config/BoolConfig")]
    public class BoolConfig : Config<bool> { }
}