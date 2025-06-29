using System;
using UnityEngine;

namespace Myth.Core
{
    public static class EventManager
    {
        #region UI
        public static Action OnPreloadEvent;
        #endregion
        public static Action<int, float> OnVolumeChanged;           // int group, float volume
        public static Action<int> OnAudioModeChanged;                     // Mono, Stereo, etc
    }
}