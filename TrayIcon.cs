using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TurboBootTray
{
    public static class TrayIcon
    {
        private static NotifyIcon tray;
        private static Config config;

        public static void Run(Config loadedConfig)
        {
            config = loadedConfig;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            tray = new NotifyIcon
            {
                Text = "TurboBootTray 正在运行",
                Icon = SystemIcons.Application,
                Visible = true,
                ContextMenuStrip = BuildMenu()
            };

            Application.Run();
        }

        private static ContextMenuStrip BuildMenu()
        {
            var menu = new ContextMenuStrip();

            var logItem = new ToolStripMenuItem("📖 查看日志");
            logItem.Click += (s, e) => OpenLogFile();

            var restartItem = new ToolStripMenuItem("🔄 重启所有程序");
            restartItem.Click += (s, e) => RestartAll();

            var statusItem = new ToolStripMenuItem("🧠 查看状态");
            statusItem.Click += (s, e) => ShowStatus();

            var exitItem = new ToolStripMenuItem("❌ 退出");
            exitItem.Click += (s, e) =>
            {
                Logger.Log("用户点击退出，程序关闭");
                Watcher.StopAll();
                tray.Visible = false;
                Application.Exit();
            };

            menu.Items.Add(logItem);
            menu.Items.Add(restartItem);
            menu.Items.Add(statusItem);
            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add(exitItem);

            return menu;
        }

        private static void OpenLogFile()
        {
            try
            {
                if (File.Exists(Logger.CurrentLogPath))
                    Process.Start("explorer.exe", Logger.CurrentLogPath);
                else
                    MessageBox.Show("日志文件不存在", "提示");
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法打开日志文件：" + ex.Message, "错误");
            }
        }

        private static void RestartAll()
        {
            Logger.Log("🌀 用户请求重启所有程序");

            Watcher.StopAll();

            var allTasks = config.Programs
                .Where(p => p.Trigger == "boot" || p.Trigger == "post_boot" || p.Trigger == "explorer")
                .ToList();

            TaskScheduler.LaunchTasks(allTasks);
        }

        private static void ShowStatus()
        {
            var lines = Watcher.GetStatusLines();
            string msg = lines.Count == 0 ? "暂无程序在守护中。" : string.Join("\n", lines);
            MessageBox.Show(msg, "程序运行状态");
        }
    }
}
