using System;
using System.Windows.Forms;

namespace TurboBootTray
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Logger.Log("TurboBootTray 启动中...");

            Launcher.WaitForExplorer();

            System.Threading.Thread.Sleep(1000);

            Launcher.TryLaunch(Config.ClashPath, "Clash Verge");
            Launcher.TryLaunch(Config.TBPath, "TranslucentTB");

            Logger.Log("启动完毕，初始化托盘图标...");
            TrayIcon.Run();
        }
    }
}
