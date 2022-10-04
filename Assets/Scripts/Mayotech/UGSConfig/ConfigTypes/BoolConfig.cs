using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoolConfig", menuName = "Config/Basic/BoolConfig")]
    public abstract class BoolConfig : Config<bool>
    {
        protected override void DeserializeData(JToken data)
        {
            Data = data.Value<bool>();
        }
    }
}