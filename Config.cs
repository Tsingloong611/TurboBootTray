// Config.cs
using System.Collections.Generic;

namespace TurboBootTray
{
    public class Config
    {
        public List<LaunchProgram> Programs { get; set; } = new();
        public string LogFile { get; set; } = "";
    }

    public class LaunchProgram
    {
        public string Name { get; set; } = "";
        public string Path { get; set; } = "";
        public string Trigger { get; set; } = "boot"; // boot, post_boot, after_all
        public int Delay { get; set; } = 0;           // 毫秒
        public bool Watch { get; set; } = false;      // 是否守护进程
    }
}
