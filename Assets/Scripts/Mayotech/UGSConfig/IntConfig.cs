using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "IntConfig", menuName = "Config/IntConfig")]
    public class IntConfig : Config<int>
    {
        protected override void DeserializeData(string data)
        {
            Debug.Log($"JObj: {data.ToString()}");
        }
    }
}