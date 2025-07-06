// ConfigManager.cs
using System;
using System.IO;
using System.Text.Json;

namespace TurboBootTray
{
    public static class ConfigManager
    {
        private static readonly string DefaultConfigPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "config.json"
        );

        public static Config Load(string? path = null)
        {
            path ??= DefaultConfigPath;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"找不到配置文件: {path}");
            }

            var json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true   // ✅ 添加这一行
            };
            
            return JsonSerializer.Deserialize<Config>(json, options) ?? new Config();

        }
    }
}
