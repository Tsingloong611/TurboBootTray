using System.Collections.Generic;
using System.Linq;

namespace TurboBootTray
{
    public static class Watcher
    {
        private static readonly List<WatcherThread> activeWatchers = new();

        public static void StartWatching(LaunchProgram prog)
        {
            var wt = new WatcherThread(prog);
            activeWatchers.Add(wt);
            wt.Start();
        }

        public static void StopAll()
        {
            foreach (var wt in activeWatchers)
            {
                wt.TokenSource.Cancel(); // 通知线程退出
            }
            activeWatchers.Clear();
        }

        public static List<string> GetStatusLines()
        {
            return activeWatchers
                .Select(w => $"{w.Program.Name} => {(Launcher.IsRunning(w.Program.Path) ? "运行中" : "未运行")}")
                .ToList();
        }
    }
}
