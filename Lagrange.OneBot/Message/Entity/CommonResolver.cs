namespace Lagrange.OneBot.Message.Entity;

public static class CommonResolver
{
    private static readonly HttpClient Client = new();

    public static byte[]? Resolve(string url)
    {
        if (url.StartsWith("base64://")) return Convert.FromBase64String(url.Replace("base64://", ""));

        Uri uri = new(url);

        return uri.Scheme switch
        {
            "http" or "https" => Client.GetAsync(uri).Result.Content.ReadAsByteArrayAsync().Result,
            "file" => File.ReadAllBytes(Path.GetFullPath(uri.LocalPath)),
            _ => null,
        };
    }

    public static Stream? ResolveStream(string url)
    {
        if (url.StartsWith("base64://")) return new MemoryStream(Convert.FromBase64String(url.Replace("base64://", "")));

        Uri uri = new(url);

        return uri.Scheme switch
        {
            "http" or "https" => Client.GetAsync(uri).Result.Content.ReadAsStreamAsync().Result,
            "file" => new FileStream(Path.GetFullPath(uri.LocalPath), FileMode.Open),
            _ => null,
        };
    }
}