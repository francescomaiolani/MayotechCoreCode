using System;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringConfig", menuName = "Config/StringConfig")]
    public class StringConfig : Config<string> { }
}