using UnityEngine;

[CreateAssetMenu(fileName = "PersistentFloat", menuName = "ScriptableVariable/PersistentFloat")]
public class PersistentFloat : ScriptableVariable<float>
{
    [SerializeField] protected string key;
    
    public override float Value
    {
        get
        {
            value = PlayerPrefs.GetFloat(key, DefaultValue);
            return value;
        }
        set
        {
            this.value = value;
            PlayerPrefs.SetFloat(key, value);
        }
    }
}