namespace TurboBootTray
{
    internal static class Config
    {
        public static readonly string ClashPath = @"D:\Program Files\Clash Verge\clash-verge.exe";
        public static readonly string TBPath = @"D:\Program Files\TranslucentTB-portable-x64\TranslucentTB.exe";

        public static readonly string LogFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "TurboBootTray.log"
        );
    }
}
