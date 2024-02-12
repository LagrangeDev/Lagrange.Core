namespace Lagrange.OneBot.Message.Entity;

public static class CommonResolver
{
    private static readonly HttpClient Client = new();
    
    public static byte[]? Resolve(string url)
    {
        if (url.StartsWith("http"))
        {
            return Client.GetAsync(url).Result.Content.ReadAsByteArrayAsync().Result;
        }
                
        if (url.StartsWith("file"))
        {
            string path = new Uri(url).LocalPath;
            return File.ReadAllBytes(path);
        }
                
        if (url.StartsWith("base64"))
        {
            string base64 = url.Replace("base64://", "");
            return Convert.FromBase64String(base64);
        }

        return null;
    }
}