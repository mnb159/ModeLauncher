namespace ModeLauncher.Models
{
    public class LauncherConfig
    {
        public int CountdownSeconds { get; set; } = 5;
        public string? DefaultModeId { get; set; }
        public List<LaunchMode> Modes { get; set; } = new();
    }

    public class LaunchMode
    {
        public string Id { get; set; } = System.Guid.NewGuid().ToString();
        public string Label { get; set; } = "";
        public string? Subtitle { get; set; }
        public string ExecutablePath { get; set; } = "";
        public string? Arguments { get; set; }
    }
}
