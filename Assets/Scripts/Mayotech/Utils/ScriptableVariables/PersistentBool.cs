using UnityEngine;

[CreateAssetMenu(fileName = "PersistentBool", menuName = "ScriptableVariable/PersistentBool")]
public class PersistentBool: ScriptableVariable<bool>
{
    [SerializeField] protected string key;
    
    public override bool Value
    {
        get
        {
            value = PlayerPrefs.GetInt(key, 0) != 0;
            return value;
        }
        set
        {
            this.value = value;
            PlayerPrefs.SetInt(key, value ? 1 : 0);
        }
    }
}