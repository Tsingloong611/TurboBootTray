// Program.cs
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace TurboBootTray
{
    internal static class Program
    {
        static Config config;

        [STAThread]
        static void Main()
        {
            config = ConfigManager.Load();
            Logger.Init(config.LogFile);

            Logger.Log($"已加载配置项 {config.Programs.Count} 个");
            foreach (var p in config.Programs)
                Logger.Log($"➡ 配置程序：{p.Name}, Trigger={p.Trigger}, Watch={p.Watch}");


            Logger.Log("TurboBootTray 启动中...");

            var bootTasks = config.Programs.Where(p => p.Trigger == "boot").ToList();
            var postBootTasks = config.Programs.Where(p => p.Trigger == "post_boot").ToList();

            TaskScheduler.LaunchTasks(bootTasks);

            WaitForExplorer();
            Thread.Sleep(1000);

            TaskScheduler.LaunchTasks(postBootTasks);

            Logger.Log("启动完毕，初始化打盘图标...");
            RunTrayIcon();
        }

        static void WaitForExplorer()
        {
            Logger.Log("等待 explorer.exe 加载...");
            while (Process.GetProcessesByName("explorer").Length == 0)
                Thread.Sleep(1000);
            Logger.Log("explorer.exe 已加载");
        }

        static void RunTrayIcon()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            NotifyIcon tray = new NotifyIcon
            {
                Text = "TurboBootTray 正在运行",
                Icon = SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = new ContextMenuStrip()
            };

            var exitItem = new ToolStripMenuItem("退出");
            exitItem.Click += (s, e) =>
            {
                Logger.Log("用户点击退出，程序关闭");
                tray.Visible = false;
                Application.Exit();
            };

            tray.ContextMenuStrip.Items.Add(exitItem);
            Application.Run();
        }
    }
}
