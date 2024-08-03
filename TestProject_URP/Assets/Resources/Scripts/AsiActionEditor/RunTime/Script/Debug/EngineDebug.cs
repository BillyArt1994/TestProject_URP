using UnityEngine;

namespace AsiActionEngine.RunTime
{
    public partial class EngineDebug
    {
        public static void Log(string _string)
        {
#if UNITY_EDITOR
            Debug.Log(_string);
#endif
        }
        public static void LogWarning(string _string)
        {
#if UNITY_EDITOR
            Debug.LogWarning(_string);
#endif
        }
        public static void LogError(string _string)
        {
#if UNITY_EDITOR
            Debug.LogError(_string);
#endif
        }
    }
}