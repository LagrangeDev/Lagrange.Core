using System.Net;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Lagrange.Core.Common;
using Lagrange.Milky.Configuration;
using Microsoft.Extensions.Options;

namespace Lagrange.Milky.Utility;

public sealed class Signer : BotSignProvider, IDisposable
{
    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "WhiteListCommand")]
    private static extern ref HashSet<string> GetPCWhiteListCommand([UnsafeAccessorType("Lagrange.Core.Common.DefaultBotSignProvider, Lagrange.Core")] object? _);

    private readonly string _url;
    private readonly HttpClient _client;

    private readonly long _uin;
    private readonly string? _token;

    public Signer(IOptions<CoreConfiguration> options)
    {
        var signerConfiguration = options.Value.Signer;
        _url = signerConfiguration.Url ?? throw new Exception("Core.Signer.Url cannot be null");
        _client = new HttpClient(new HttpClientHandler
        {
            Proxy = signerConfiguration.ProxyUrl == null ? null : new WebProxy
            {
                Address = new Uri(signerConfiguration.ProxyUrl),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
            },
        });

        _uin = options.Value.Login.Uin ?? 0;
        _token = signerConfiguration.Token;
    }

    public override bool IsWhiteListCommand(string cmd) => GetPCWhiteListCommand(null).Contains(cmd);

    public override async Task<SsoSecureInfo?> GetSecSign(long uin, string cmd, int seq, ReadOnlyMemory<byte> body)
    {
        using var request = new HttpRequestMessage();
        request.Method = HttpMethod.Post;
        request.RequestUri = new Uri($"{_url}{(_url.EndsWith('/') ? "" : "/")}api/sign/sec-sign");
        if (!string.IsNullOrEmpty(_token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }
        request.Content = new StringContent(
            JsonUtility.Serialize(new SecSignRequest
            {
                Uin = uin == 0 ? _uin : uin,
                Command = cmd,
                Sequence = seq,
                Body = Convert.ToHexString(body.Span).ToLower(),
                Guid = Convert.ToHexString(Context.Keystore.Guid).ToLower(),
                Qua = Context.AppInfo.Qua,
            }),
            System.Text.Encoding.UTF8,
            MediaTypeNames.Application.Json
        );

        using var response = await _client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        using var stream = await response.Content.ReadAsStreamAsync();
        var result = JsonUtility.Deserialize<SignerResponse<SecSignResponse>>(stream);
        if (result == null) throw new Exception("Signer response serialization failed");
        if (result.Code != 0) throw new Exception($"Signer server exception: ({result.Code}){result.Message}");

        return new SsoSecureInfo
        {
            SecSign = Convert.FromHexString(result.Value.SecSign),
            SecToken = Convert.FromHexString(result.Value.SecToken),
            SecExtra = Convert.FromHexString(result.Value.SecExtra),
        };
    }


    public void Dispose()
    {
        _client.Dispose();
    }
}

public class SignerResponse<T>
{
    [JsonPropertyName("code")]
    public required int Code { get; init; }

    [JsonPropertyName("message")]
    public required string? Message { get; init; }

    [JsonPropertyName("value")]
    public required T Value { get; init; }
}

public class SecSignRequest
{
    [JsonPropertyName("uin")]
    public required long Uin { get; init; }

    [JsonPropertyName("command")]
    public required string Command { get; init; }

    [JsonPropertyName("seq")]
    public required int Sequence { get; init; }

    [JsonPropertyName("body")]
    public required string Body { get; init; }

    [JsonPropertyName("guid")]
    public required string Guid { get; init; }

    [JsonPropertyName("qua")]
    public required string Qua { get; init; }
}

public class SecSignResponse
{
    [JsonPropertyName("sec_sign")]
    public required string SecSign { get; init; }

    [JsonPropertyName("sec_token")]
    public required string SecToken { get; init; }

    [JsonPropertyName("sec_extra")]
    public required string SecExtra { get; init; }
}