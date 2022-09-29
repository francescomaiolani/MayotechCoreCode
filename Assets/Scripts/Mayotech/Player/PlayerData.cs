using System;
using UnityEngine;

namespace Mayotech.Player
{
    [Serializable]
    public abstract class PlayerData : ScriptableObject
    {
        [SerializeField] protected StringVariable mappedVariable;

        public StringVariable MappedVariable => mappedVariable;
    }
    [Serializable]
    public abstract class PlayerData<T> : PlayerData
    {
        [SerializeField] protected T data;
        
        public T Data
        {
            get => data;
            set => data = value;
        }
    }
}
