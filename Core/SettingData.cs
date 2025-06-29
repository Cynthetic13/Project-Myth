using System.Collections.Generic;

namespace Myth.Core
{
    [System.Serializable]
    public class SettingData
    {
        // Audio Settings
        public Dictionary<string, float> VolumeData = new();
    }
}