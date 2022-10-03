using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "FloatConfig", menuName = "Config/FloatConfig")]
    public class FloatConfig : Config<float>
    {
        protected override void DeserializeData(string data)
        {
            Debug.Log($"JObj: {data.ToString()}");

        }
    }
}