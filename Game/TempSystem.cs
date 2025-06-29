using UnityEngine;

namespace Myth.Core
{
    public static class TempSystem
    {
        public static TempType tempType = TempType.Celsius;
        public static float currentTemp;
        
        public const float TEMP_HOT_CAUTION = 27;
        public const float TEMP_HOT_EXTREME_CAUTION = 32;
        public const float TEMP_HOT_DANGER = 40;
        public const float TEMP_HOT_EXTREME_DANGER = 52;

        public const float TEMP_COLD_CAUTION = 0;
        public const float TEMP_COLD_EXTREME_CAUTION = -10;
        public const float TEMP_COLD_DANGER = -15;
        public const float TEMP_COLD_EXTREME_DANGER = -30;

        public static void SetTemp(float temp) => currentTemp = temp;

        private static float ConvertTempToFahrenheit(float tempInCelsius)
        {
            return (tempInCelsius * 9 / 5) + 32;
        }

        public enum TempType
        {
            Celsius,
            Fahrenheit
        }
    }
}