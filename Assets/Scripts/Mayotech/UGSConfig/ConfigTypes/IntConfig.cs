using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntConfig", menuName = "Config/Basic/IntConfig")]
    public class IntConfig : Config<int>
    {
        protected override void DeserializeData(JToken data)
        {
            Data = data.Value<int>();
        }
    }
}