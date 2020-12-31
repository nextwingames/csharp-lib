using System;
using UnityEngine;

namespace Nextwin.Util
{
    public static class Print
    {
        public static void Log()
        {
            Log("", 2);
        }

        public static void Log<T>(T message)
        {
            Log(message, 2);
        }

        public static void Log(byte[] bytes)
        {
            Log(ArrayStringConverter.BytesToString(bytes), 2);
        }

        public static void LogError<T>(T message)
        {
            LogError(message, 2);
        }

        private static void Log<T>(T message, int stackFrame)
        {
            try
            {
                Debug.Log(message);
            }
            catch(Exception)
            {
                var frame = new System.Diagnostics.StackFrame(stackFrame);
                string format = $"[{DateTime.Now:HH:mm:ss}] {message}\n{frame.GetMethod().DeclaringType.Name}\n";
                Console.WriteLine(format);
            }
        }

        private static void LogError<T>(T message, int stackFrame)
        {
            try
            {
                Debug.LogError(message);
            }
            catch(Exception)
            {
                
            }
        }
    }
}
