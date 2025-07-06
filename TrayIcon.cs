using System;
using System.Windows.Forms;

namespace TurboBootTray
{
    internal static class TrayIcon
    {
        public static void Run()
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
