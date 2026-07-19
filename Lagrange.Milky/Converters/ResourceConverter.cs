using System;
using System.Buffers.Text;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Converters;

public class ResourceConverter
{
    private readonly HttpClient _http = new();

    public ValueTask<Stream> UriToStreamAsync(string uri, CancellationToken ct) => uri switch
    {
        _ when uri.StartsWith("file://") => FileUriToStreamAsync(uri, ct),
        _ when uri.StartsWith("http://") || uri.StartsWith("https://") => HttpToStreamAsync(uri, ct),
        _ when uri.StartsWith("base64://") => Base64ToStreamAsync(uri),
        _ => throw new NotSupportedException(),
    };

    private static ValueTask<Stream> FileUriToStreamAsync(string uri, CancellationToken ct)
    {
        return ValueTask.FromResult<Stream>(File.OpenRead(new Uri(uri).LocalPath));
    }

    private async ValueTask<Stream> HttpToStreamAsync(string uri, CancellationToken ct)
    {
        var response = await _http.GetAsync(uri, ct);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStreamAsync(ct);
    }

    private ValueTask<Stream> Base64ToStreamAsync(string uri)
    {
        var base64 = uri.AsSpan("base64://".Length);
        byte[] bytes = new byte[(int)(4 * Math.Ceiling((decimal)(base64.Length / 3)))];
        if (!Convert.TryFromBase64Chars(base64, bytes, out _))
        {
            throw new FormatException("The 'base64://' URI does not contain valid Base64 data.");
        }
        return ValueTask.FromResult<Stream>(new MemoryStream(bytes, writable: false));
    }
}