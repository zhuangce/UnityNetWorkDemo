using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

//namespace MyDebug
//{
    public static class DebugTool 
    {

       

        [Conditional("DebugControl")]
        public static void Log(object message, UnityEngine.Object obj = null)
        {
            UnityEngine.Debug.Log("<color=green>[Type]</color>--"+message, obj);
        }

        [Conditional("DebugControl")]
        public static void LogWarning(object message, UnityEngine.Object obj = null)
        {
            UnityEngine.Debug.LogWarning("<color=yellow>[Type]</color>--"+message, obj);
        }

        [Conditional("DebugControl")]
        public static void LogError(object message, UnityEngine.Object obj = null)
        {
            UnityEngine.Debug.LogError("<color=red>[Type]</color>--"+message, obj);
        }
    }
//}
