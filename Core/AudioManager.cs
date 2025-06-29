using System.Collections.Generic;
using Myth.Misc;
using UnityEngine;
using UnityEngine.Audio;

namespace Myth.Core
{
    // TODO... Remove all Setting Data setting and loading from here and pass it into a function making this a static class
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer mixer;
        private List<AudioMixerGroup> _mixerGroup = new();
        private List<float> _volume = new();
        private AudioMixerGroup[] _mixerGroups;
        private SettingData _settingData;

        private void OnEnable()
        {
            EventManager.OnAudioModeChanged += ModeChangeEvent;
        }

        private void OnDisable()
        {
            EventManager.OnAudioModeChanged -= ModeChangeEvent;
        }

        private void Start() 
        {
            // Get the PlayerPrefs JSON
            _settingData = new SettingData();
            _settingData.VolumeData = new Dictionary<string, float>();
            
            _mixerGroups = mixer.FindMatchingGroups("Master");
            
            // Load Volumes from file
            _settingData = SaveSystem.LoadSettings();
            for (int i = 0; i < _mixerGroups.Length; i++)
            {
                _settingData.VolumeData.TryGetValue(_mixerGroups[i].name, out float volume);
                PopulateLists(i, volume);
                SetVolume(volume, i);
            }
        }

        public void IncreaseVolume(int group)
        {
            // Get the current volume
            float currentVolume = _volume[group];
            
            // Check if volume is already at 100 and return early
            if (Mathf.Approximately(currentVolume, 100f)) return;

            // Add 5 to the volume
            currentVolume += 5f;
            
            // Set Volume
            SetVolume(currentVolume, group);
        }

        public void DecreaseVolume(int group)
        {
            // Get the current volume
            float currentVolume = _volume[group];
                
            // Check if volume is already at 0 and return early
            if (Mathf.Approximately(currentVolume, 0f)) return;

            // Add 5 to the volume
            currentVolume -= 5f;

            // Set Volume
            SetVolume(currentVolume, group);
        }

        private void SetVolume(float currentVolume, int group)
        {
            // Convert to DB
            float dbVolume = VolumeToDB(currentVolume);

            // Call the event
            EventManager.OnVolumeChanged?.Invoke(group, currentVolume);

            // Set MixerGroup List and Volume List to reflect changes
            _mixerGroup[group].audioMixer.SetFloat(_mixerGroup[group].name, dbVolume);
            _volume[group] = currentVolume;
            
            // Set Settings Data
            _settingData.VolumeData[_mixerGroup[group].name] = currentVolume;
            
            // Save to Settings JSON
            SaveSystem.SaveSettings(_settingData);
        }

        private void PopulateLists(int i, float volume = 100.0f)
        {
            _mixerGroup.Add(_mixerGroups[i]);
            _volume.Add(volume);
        }

        private float VolumeToDB(float volumePercent)
        {
            if (volumePercent <= 0f)
                return -80f;
            
            return Mathf.Log10(volumePercent / 100.0f) * 20.0f;
        }

        private void ModeChangeEvent(int value)
        {
            AudioConfiguration config = AudioSettings.GetConfiguration();

            config.speakerMode = (AudioSpeakerMode)value;

            AudioSettings.Reset(config);
        }
    }
}