using System;
using UnityEngine;

[Serializable]
public class ScriptableVariable<T> : ScriptableObject
{
    [SerializeField] private T value;

    public T Value
    {
        get => value;
        set => this.value = value;
    }
}
