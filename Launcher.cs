using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace TurboBootTray
{
    internal static class Launcher
    {
        public static void WaitForExplorer()
        {
            Logger.Log("等待 explorer.exe 加载...");
            while (Process.GetProcessesByName("explorer").Length == 0)
            {
                Thread.Sleep(1000);
            }
            Logger.Log("explorer.exe 已加载");
        }

        public static void TryLaunch(string path, string name)
        {
            try
            {
                if (!File.Exists(path))
                {
                    Logger.Log($"❌ 未找到 {name}：{path}");
                    return;
                }

                string exeName = Path.GetFileNameWithoutExtension(path).ToLower();
                if (Process.GetProcessesByName(exeName).Any())
                {
                    Logger.Log($"⚠️ {name} 已在运行中，跳过启动。");
                    return;
                }

                Process.Start(path);
                Logger.Log($"✅ 启动 {name} 成功");
            }
            catch (Exception ex)
            {
                Logger.Log($"❌ 启动 {name} 失败：{ex.Message}");
            }
        }
    }
}
