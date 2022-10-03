using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "BoolConfig", menuName = "Config/BoolConfig")]
    public class BoolConfig : Config<bool>
    {
        protected override void DeserializeData(string data)
        {
            Debug.Log($"JObj: {data.ToString()}");
        }
    }
}