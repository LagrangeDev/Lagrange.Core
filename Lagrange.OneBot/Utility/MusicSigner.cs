using Lagrange.OneBot.Message.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Nodes;

namespace Lagrange.OneBot.Utility;

public class MusicSigner
{
    private static string? _signServer;

    private readonly static HttpClient _client = new();

    public MusicSigner(IConfiguration config, ILogger<MusicSigner> logger)
    {
        _signServer = config["MusicSignServerUrl"] ?? "";

        if (string.IsNullOrEmpty(_signServer))
        {
            logger.LogWarning("MusicSignServer is not available, sign may be failed");
        }
        else
        {
            logger.LogInformation("MusicSignServer Service is successfully established");
        }
    }


    public static string? Sign(MusicSegment musicSegment)
    {
        if (string.IsNullOrEmpty(_signServer)) return null;

        var payload = new JsonObject()
        {
            { "type" , musicSegment.Type },
            { "url" , musicSegment.Url },
            { "audio" , musicSegment.Audio },
            { "title" , musicSegment.Title },
            { "image" , musicSegment.Image },
            { "singer" , musicSegment.Singer },
        };

        try
        {
            var message = _client.PostAsJsonAsync(_signServer, payload).Result;
            return message.Content.ReadAsStringAsync().Result;
        }
        catch
        {
            return null;
        }
    }
}
