using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringConfig", menuName = "Config/Basic/StringConfig")]
    public abstract class StringConfig : Config<string>
    {
        protected override void DeserializeData(JToken data)
        {
            Data = data.Value<string>();
        }
    }
}