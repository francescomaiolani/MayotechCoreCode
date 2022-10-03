using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    [CreateAssetMenu(fileName = "StringConfig", menuName = "Config/StringConfig")]
    public class StringConfig : Config<string>
    {
        protected override void DeserializeData(string data)
        {
            Debug.Log($"JObj: {data.ToString()}");
        }
    }
}