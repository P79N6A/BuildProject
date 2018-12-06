using UnityEngine;

namespace Sakura
{
    public class DebugX
    {
        public static bool enabled = true;

        public static void Log(string message, params object[] args)
        {
            if (!enabled || string.IsNullOrEmpty(message))
            {
                return;
            }

            string msg = SAStringUtils.Substitute(message, args);
            Debug.Log(msg);
        }

        public static void LogWarning(string message, params object[] args)
        {
            if (!enabled || string.IsNullOrEmpty(message))
            {
                return;
            }

            string msg = SAStringUtils.Substitute(message, args);
            Debug.LogWarning(msg);
        }
    }
}