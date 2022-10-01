using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionsManager", menuName = "Manager/OptionsManager")]
public class OptionsManager : Service
{
    [SerializeField] protected PersistentBool audioSetting;
    [SerializeField] protected PersistentBool soundSetting;
    [SerializeField] protected PersistentFloat volumeSetting;

    public override void InitService()
    {
        
    }

    public bool IsAudioOn => audioSetting.Value;
    public bool IsSoundOn => soundSetting.Value;
    public float VolumeValue => volumeSetting.Value;
}