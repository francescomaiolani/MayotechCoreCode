using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    public abstract class JSONConfig<T> : Config<string>
    {
        [SerializeField] protected GameEvent<string> onConfigLoaded;
        [SerializeField] protected T deserializedData;

        public override string Data
        {
            get => data;
            set
            {
                data = value;
                onConfigLoaded?.RaiseEvent(data);
                DeserializeData(data);
            }
        }

        public T DeserializedData
        {
            get => deserializedData;
            set => deserializedData = value;
        }

        public void DeserializeData(string data)
        {
            DeserializedData = JsonConvert.DeserializeObject<T>(data);
        }
    }
}