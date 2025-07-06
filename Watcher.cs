using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace TurboBootTray
{
    public static class Watcher
    {
        public static void StartWatching(LaunchProgram prog)
        {
            Thread thread = new Thread(() =>
            {
                string exeName = Path.GetFileNameWithoutExtension(prog.Path).ToLower();
                while (true)
                {
                    try
                    {
                        bool isRunning = Process.GetProcessesByName(exeName).Any();
                        if (!isRunning)
                        {
                            Logger.Log($"🔄 守护进程触发：{prog.Name} 未在运行，尝试重启...");
                            Process.Start(prog.Path);
                            Logger.Log($"✅ 已尝试重启 {prog.Name}");
                            Thread.Sleep(3000); // 避免频繁重启
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Log($"❌ 守护 {prog.Name} 时出错：{ex.Message}");
                    }

                    Thread.Sleep(5000); // 每5秒检查一次
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
