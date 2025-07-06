using System;
using System.IO;

namespace TurboBootTray
{
    internal static class Logger
    {
        public static void Log(string msg)
        {
            string fullMsg = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}{Environment.NewLine}";
            File.AppendAllText(Config.LogFile, fullMsg);
        }
    }
}
