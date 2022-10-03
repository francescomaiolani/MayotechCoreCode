using System;
using Mayotech.UGSConfig;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerData : ConfigData
{
    [JsonProperty("playerID" )]
    [SerializeField] private string playerID;
    [JsonProperty("sessionToken")]
    [SerializeField] private string sessionToken;
}