namespace Lagrange.OneBot.Message.Entity;

public static class CommonResolver
{
    private static readonly HttpClient Client = new();

    public static async Task<byte[]?> ResolveAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            if (url.StartsWith("base64://"))
                return Convert.FromBase64String(url.Substring("base64://".Length));

            Uri uri = new(url);

            return uri.Scheme switch
            {
                "http" or "https" => await (await Client.GetAsync(uri, cancellationToken)).Content.ReadAsByteArrayAsync(cancellationToken),
                "file" => await File.ReadAllBytesAsync(Path.GetFullPath(uri.LocalPath), cancellationToken),
                _ => null,
            };
        }
        catch (Exception ex) when (ex is HttpRequestException or FileNotFoundException or DirectoryNotFoundException)
        {
            return null;
        }
    }

    public static async Task<Stream?> ResolveStreamAsync(string url, CancellationToken cancellationToken = default)
    {
        try
        {
            if (url.StartsWith("base64://"))
                return new MemoryStream(Convert.FromBase64String(url.Substring("base64://".Length)));

            Uri uri = new(url);

            return uri.Scheme switch
            {
                "http" or "https" => await (await Client.GetAsync(uri, cancellationToken)).Content.ReadAsStreamAsync(cancellationToken),
                "file" => new FileStream(Path.GetFullPath(uri.LocalPath), FileMode.Open, FileAccess.Read, FileShare.Read),
                _ => null,
            };
        }
        catch (Exception ex) when (ex is HttpRequestException or FileNotFoundException or DirectoryNotFoundException)
        {
            return null;
        }
    }

    public static byte[]? Resolve(string url)
    {
        if (url.StartsWith("base64://"))
            return Convert.FromBase64String(url.Substring("base64://".Length));

        Uri uri = new(url);

        return uri.Scheme switch
        {
            "file" => File.ReadAllBytes(Path.GetFullPath(uri.LocalPath)),
            "http" or "https" => ResolveAsync(url).GetAwaiter().GetResult(),
            _ => null,
        };
    }

    public static Stream? ResolveStream(string url)
    {
        if (url.StartsWith("base64://"))
            return new MemoryStream(Convert.FromBase64String(url.Substring("base64://".Length)));

        Uri uri = new(url);

        return uri.Scheme switch
        {
            "file" => new FileStream(Path.GetFullPath(uri.LocalPath), FileMode.Open, FileAccess.Read, FileShare.Read),
            "http" or "https" => ResolveStreamAsync(url).GetAwaiter().GetResult(),
            _ => null,
        };
    }
}