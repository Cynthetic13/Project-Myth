using System.IO;
using UnityEngine;
using Newtonsoft.Json;

namespace Myth.Core
{
    /// <summary>
    /// A Save System for the Settings Data
    /// </summary>
    public static class SaveSystem
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "config.json");

        public static void SaveSettings(SettingData data)
        {
            string json = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        public static SettingData LoadSettings()
        {
            if (SaveFileExists())
            {
                string json = File.ReadAllText(SavePath);
                return JsonConvert.DeserializeObject<SettingData>(json);
            }
            else
            {
                SettingData defaultData = CreateDefaultSettingData();
                SaveSettings(defaultData);
                
                return defaultData;
            }
        }

        private static SettingData CreateDefaultSettingData()
        {
            SettingData data = new SettingData
            {
                VolumeData =
                {
                    ["Master"] = 100f,
                    ["Music"] = 100f,
                    ["Blocks"] = 100f,
                    ["Ambient"] = 100f,
                    ["Hostile Mobs"] = 100f,
                    ["Friendly Mobs"] = 100f,
                    ["UI"] = 100f,
                    ["Player"] = 100f
                }
            };

            return data;
        }

        private static bool SaveFileExists() => File.Exists(SavePath);
    }
}