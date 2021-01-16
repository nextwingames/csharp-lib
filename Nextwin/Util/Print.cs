using System;

namespace Nextwin.Util
{
    public enum ELog
    {
        Log,
        Warning,
        Error,
        None
    }

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
            Log(message, 2, ELog.Error);
        }

        public static void LogWarning<T>(T message)
        {
            Log(message, 2, ELog.Warning);
        }

        private static void Log<T>(T message, int stackFrame, ELog logType = ELog.Log)
        {
            ChangeConsoleForegroundColor(logType);

            var frame = new System.Diagnostics.StackFrame(stackFrame);
            string format = $"[{DateTime.Now:HH:mm:ss}] {message}\n[{logType}] - {frame.GetMethod().DeclaringType.Name}\n";
            Console.WriteLine(format);

            ChangeConsoleForegroundColor(ELog.None);
        }

        private static void ChangeConsoleForegroundColor(ELog logType)
        {
            switch(logType)
            {
                case ELog.Log:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case ELog.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case ELog.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case ELog.None:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
            }
        }
    }
}
