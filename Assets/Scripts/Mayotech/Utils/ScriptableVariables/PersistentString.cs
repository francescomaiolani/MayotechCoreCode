using UnityEngine;

[CreateAssetMenu(fileName = "PersistentString", menuName = "ScriptableVariable/PersistentString")]
public class PersistentString: ScriptableVariable<string>
{
    [SerializeField] protected string key;
    
    public override string Value
    {
        get
        {
            value = PlayerPrefs.GetString(key, string.Empty);
            return value;
        }
        set
        {
            this.value = value;
            PlayerPrefs.SetString(key, value);
        }
    }
}