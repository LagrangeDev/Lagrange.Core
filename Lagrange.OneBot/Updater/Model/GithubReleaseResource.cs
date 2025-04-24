namespace Lagrange.OneBot.Updater.Model
{
    public class GithubReleaseResource
    {
        public string Url { get; set; } = string.Empty;
        public DotNetVersion DotNetVersion { get; set; }
        public RuntimeIdentifier RuntimeIdentifier { get; set; }
        public CompressedFormat CompressedFormat { get; set; }
        
    }
}