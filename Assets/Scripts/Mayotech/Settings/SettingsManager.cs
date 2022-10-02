using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mayotech.Settings
{
    [CreateAssetMenu(fileName = "SettingsManager", menuName = "Manager/SettingsManager")]
    public class SettingsManager : Service
    {
        [Header("Settings")]
        [SerializeField] protected PersistentBool musicSetting;
        [SerializeField] protected PersistentBool soundSetting;
        [SerializeField] protected PersistentFloat volumeSetting;

        [Header("Game Events")] 
        [SerializeField] private GameEvent<bool> onMusicChanged;
        [SerializeField] private GameEvent<bool> onSoundChanged;
        [SerializeField] private GameEvent<float> onVolumeChanged;
        
        public override void InitService()
        {
        
        }

        public bool IsAudioOn => musicSetting.Value;
        public bool IsSoundOn => soundSetting.Value;
        public float VolumeValue => volumeSetting.Value;

        public void SetMusic(bool on)
        {
            musicSetting.Value = on;
            onMusicChanged?.RaiseEvent(on);
        }
        
        public void SetSound(bool on)
        {
            soundSetting.Value = on;
            onSoundChanged?.RaiseEvent(on);
        }
        
        public void SetVolume(float volume)
        {
            volumeSetting.Value = volume;
            onVolumeChanged?.RaiseEvent(volume);
        }
    }
}
