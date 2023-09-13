using System;

namespace Milo.Utilities
{
    public class TimeUtils
    {
        public static string SecondsToString_MS(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return string.Format(@"{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        }

        public static string SecondsToString_HMS(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return string.Format(@"{0:00}:{1:00}:{2:00}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }

        public static string SecondsToString_DHMS(float seconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
            return string.Format(@"{0:00}:{1:00}:{2:00}:{3:00}", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
        }
    }
}
