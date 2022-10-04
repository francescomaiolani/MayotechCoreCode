using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloatConfig", menuName = "Config/Basic/FloatConfig")]
    public abstract class FloatConfig : Config<float>
    {
        protected override void DeserializeData(JToken data)
        {
            Data = data.Value<float>();
        }
    }
}