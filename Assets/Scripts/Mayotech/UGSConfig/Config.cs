using System;
using UnityEngine;

namespace Mayotech.UGSConfig
{
    [Serializable]
    public abstract class Config : ScriptableObject
    {
        [SerializeField] protected string configKey;

        public string ConfigKey => configKey;
    }
    
    [Serializable]
    public abstract class Config<T> : Config, IConfig<T>
    {
        [SerializeField] protected T data;
        [SerializeField] protected GameEvent<T> onConfigLoaded;

        public virtual T Data
        {
            get => data;
            set
            {
                data = value;
                onConfigLoaded?.RaiseEvent(data);
            }
        }
    }
}