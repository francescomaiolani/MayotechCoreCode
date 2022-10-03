using System;
using System.Reflection;
using Newtonsoft.Json;

namespace Mayotech.UGSConfig
{
    [Serializable]
    public abstract class JSONConfig<T> : Config<T> where T : ConfigData
    {
        public override T Data
        {
            get => data;
            set => data = value;
        }

        public void SetData(string json)
        {
            Data = JsonConvert.DeserializeObject<T>(json);
        }
    }

    public class ConfigData{ }
}