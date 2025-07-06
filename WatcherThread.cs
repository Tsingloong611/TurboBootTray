using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;

namespace TurboBootTray
{
    public class WatcherThread
    {
        public LaunchProgram Program { get; }
        public Thread Thread { get; }
        public CancellationTokenSource TokenSource { get; }

        public WatcherThread(LaunchProgram program)
        {
            Program = program;
            TokenSource = new CancellationTokenSource();

            Thread = new Thread(() => Run(TokenSource.Token))
            {
                IsBackground = true
            };
        }

        public void Start() => Thread.Start();
        private void Run(CancellationToken token)
        {
            string targetExePath = Path.GetFullPath(Program.Path).ToLowerInvariant();
            string exeName = Path.GetFileName(Program.Path);

            while (!token.IsCancellationRequested)
            {
                try
                {
                    bool isRunning = false;

                    using (var searcher = new ManagementObjectSearcher(
                        $"SELECT ExecutablePath FROM Win32_Process WHERE Name = '{exeName}'"))
                    {
                        foreach (var obj in searcher.Get())
                        {
                            string? exePath = obj["ExecutablePath"]?.ToString()?.ToLowerInvariant();
                            if (exePath == targetExePath)
                            {
                                isRunning = true;
                                break;
                            }
                        }
                    }

                    if (!isRunning)
                    {
                        Logger.Log($"🔄 守护进程触发：{Program.Name} 未在运行，尝试重启...");
                        Process.Start(Program.Path);
                        Logger.Log($"✅ 已尝试重启 {Program.Name}");
                        Thread.Sleep(3000); // 防止频繁拉起
                    }
                }
                catch (Exception ex)
                {
                    Logger.Log($"❌ 守护 {Program.Name} 时出错：{ex.Message}");
                }

                Thread.Sleep(5000); // 轮询间隔
            }
        }
    }
}
