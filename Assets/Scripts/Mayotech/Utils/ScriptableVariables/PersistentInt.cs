using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentInt", menuName = "ScriptableVariable/PersistentInt")]
public class PersistentInt : ScriptableVariable<int>
{
    [SerializeField] protected string key;
    
    public override int Value
    {
        get
        {
            value = PlayerPrefs.GetInt(key, DefaultValue);
            return value;
        }
        set
        {
            this.value = value;
            PlayerPrefs.SetInt(key, value);
        }
    }
}