using ModeLauncher.Models;
using System.IO;
using System.Text.Json;

namespace ModeLauncher.Services
{
    public class ConfigService
    {
        private readonly string _configPath;

        public ConfigService()
        {
            var appData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var folder = Path.Combine(appData, "ModeLauncher");
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _configPath = Path.Combine(folder, "config.json");
        }

        public LauncherConfig Load()
        {
            if (!File.Exists(_configPath))
            {
                var defaultConfig = CreateDefault();
                Save(defaultConfig);
                return defaultConfig;
            }

            var json = File.ReadAllText(_configPath);
            return JsonSerializer.Deserialize<LauncherConfig>(json)
                   ?? CreateDefault();
        }

        public void Save(LauncherConfig config)
        {
            var json = JsonSerializer.Serialize(config, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            File.WriteAllText(_configPath, json);
        }

        private LauncherConfig CreateDefault()
        {
            var cfg = new LauncherConfig
            {
                CountdownSeconds = 5,
                DefaultModeId = "gaming"
            };

            cfg.Modes.Add(new LaunchMode
            {
                Id = "gaming",
                Label = "Gaming",
                Subtitle = "Steam Big Picture",
                ExecutablePath = @"C:\Program Files (x86)\Steam\steam.exe",
                Arguments = "-bigpicture"
            });

            cfg.Modes.Add(new LaunchMode
            {
                Id = "streaming",
                Label = "Streaming",
                Subtitle = "Jellyfin Player",
                ExecutablePath = @"C:\Program Files\Jellyfin Media Player\jellyfinmediaplayer.exe"
            });

            cfg.Modes.Add(new LaunchMode
            {
                Id = "windows",
                Label = "Windows",
                Subtitle = "Windows Desktop",
                ExecutablePath = "explorer.exe"
            });

            return cfg;
        }
    }
}
