using System;
using UnityEngine;

[Serializable]
public class ScriptableVariable<T> : ScriptableObject
{
    [SerializeField] protected T value;

    public virtual T Value
    {
        get => value;
        set => this.value = value;
    }
}
