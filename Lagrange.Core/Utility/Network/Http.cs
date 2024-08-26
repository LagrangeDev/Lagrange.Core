using System.Net.Http.Headers;
using System.Web;

namespace Lagrange.Core.Utility.Network;

internal static class Http
{
    private static readonly HttpClient Client = new();

    public static async Task<string> GetAsync(string url, Dictionary<string, string>? param = null)
    {
        var uriBuilder = new UriBuilder(url);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        if (param != null)
        {
            foreach (var (key, value) in param) query[key] = value;
        }
        uriBuilder.Query = query.ToString();

        var response = await Client.GetAsync(uriBuilder.Uri);
        return await response.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// Post form data to url, using application/x-www-form-urlencoded
    /// </summary>
    /// <param name="url"></param>
    /// <param name="payload"></param>
    /// <returns></returns>
    public static async Task<string> PostAsync(string url, Dictionary<string, string>? payload = null)
    {
        var content = new FormUrlEncodedContent(payload ?? new Dictionary<string, string>());
        var response = await Client.PostAsync(url, content);
        return await response.Content.ReadAsStringAsync();
    }

    public static async Task<byte[]> PostAsync(string url, byte[] payload, string content)
    {
        var contentData = new ByteArrayContent(payload);
        contentData.Headers.ContentType = new MediaTypeHeaderValue(content);
        var response = await Client.PostAsync(url, contentData);
        return await response.Content.ReadAsByteArrayAsync();
    }
}