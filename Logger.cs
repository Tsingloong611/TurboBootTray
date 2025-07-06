// Logger.cs
using System;
using System.IO;

namespace TurboBootTray
{
    public static class Logger
    {
        private static string logPath = "";
        public static string CurrentLogPath { get; private set; }

        public static void Init(string path)
        {
            logPath = Environment.ExpandEnvironmentVariables(path);
            CurrentLogPath = logPath;

            var dir = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(dir))
                Directory.CreateDirectory(dir);
        }


        public static void Log(string msg)
        {
            string line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}";
            File.AppendAllText(logPath, line + Environment.NewLine);
        }
    }
}
