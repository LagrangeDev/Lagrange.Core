namespace Lagrange.OneBot.Updater.Model
{
    public class UpdaterConfig
    {
        public bool EnableAutoUpdate { get; set; } = false;
        public int CheckInterval { get; set; } = 360;
        public string ProxyUrl { get; set; } = string.Empty;
        public string LastUpdateTime { get; set; } = string.Empty;
    }
}