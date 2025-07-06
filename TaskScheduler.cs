using System.Collections.Generic;
using System.Threading;

namespace TurboBootTray
{
    public static class TaskScheduler
    {
        public static void LaunchTasks(List<LaunchProgram> tasks)
        {
            foreach (var prog in tasks)
            {
                if (prog.Delay > 0)
                    Thread.Sleep(prog.Delay);

                Logger.Log($"🚀 准备启动：{prog.Name}");
                Launcher.TryLaunch(prog.Path, prog.Name);

                if (prog.Watch)
                {
                    Logger.Log($"🛡️ 启用守护进程：{prog.Name}");
                    Watcher.StartWatching(prog);
                }
            }
        }
    }
}
