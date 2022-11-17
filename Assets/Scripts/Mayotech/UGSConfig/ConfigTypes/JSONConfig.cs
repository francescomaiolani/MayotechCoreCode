using System;
using Newtonsoft.Json;

namespace Mayotech.UGSConfig
{
    [Serializable]
    public abstract class JSONConfig<T> : Config<T> where T : ConfigData
    {
        public void SetData(string json)
        {
            Data = JsonConvert.DeserializeObject<T>(json);
        }
    }

    [Serializable]
    public class ConfigData { }
}