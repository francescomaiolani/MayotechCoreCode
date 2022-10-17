using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class ScriptableVariable : ScriptableObject { }

[Serializable]
public class ScriptableVariable<T> : ScriptableVariable
{
    [SerializeField] protected T value;
    [SerializeField] protected T defaultValue;

    public virtual T Value
    {
        get => value;
        set => this.value = value;
    }

    public T DefaultValue => defaultValue;

    [Button("Set Value", ButtonSizes.Medium, ButtonStyle.FoldoutButton)]
    public void SetValue(T val) => Value = val;
}