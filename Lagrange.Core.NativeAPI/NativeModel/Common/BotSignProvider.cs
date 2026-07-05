using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Service;

namespace Lagrange.Core.NativeAPI.NativeModel.Common;

internal class LinuxSignProvider(string? signUrl) : BotSignProvider, IDisposable
{
    private const string Tag = nameof(LinuxSignProvider);

    private readonly HttpClient _client = new();

    private string Url => signUrl ?? $"https://sign.lagrangecore.org/api/sign/{Context.AppInfo.AppClientVersion}";

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "WhiteListCommand")]
    private static extern ref HashSet<string> GetPCWhiteListCommand([UnsafeAccessorType("Lagrange.Core.Common.DefaultBotSignProvider, Lagrange.Core")] object? _);

    public override bool IsWhiteListCommand(string cmd) => GetPCWhiteListCommand(null).Contains(cmd);

    public override async Task<SsoSecureInfo?> GetSecSign(long uin, string cmd, int seq, ReadOnlyMemory<byte> body)
    {
        try
        {
            var payload = new JsonObject
            {
                ["cmd"] = cmd,
                ["seq"] = seq,
                ["src"] = Convert.ToHexString(body.Span),
            };

            var response = await _client.PostAsync(Url, new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return null;

            var content = JsonHelper.Deserialize<Root>(await response.Content.ReadAsStringAsync());
            if (content == null) return null;

            return new SsoSecureInfo
            {
                SecSign = Convert.FromHexString(content.Value.Sign),
                SecToken = Convert.FromHexString(content.Value.Token),
                SecExtra = Convert.FromHexString(content.Value.Extra)
            };
        }
        catch (Exception e)
        {
            Context.LogWarning(Tag, $"Failed to get sign: {e.Message}");
            return null;
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Serializable]
    internal class Root
    {
        [JsonPropertyName("value")] public Response Value { get; set; } = new();
    }

    [Serializable]
    internal class Response
    {
        [JsonPropertyName("sign")] public string Sign { get; set; } = string.Empty;

        [JsonPropertyName("token")] public string Token { get; set; } = string.Empty;

        [JsonPropertyName("extra")] public string Extra { get; set; } = string.Empty;
    }
}

internal class AndroidSignProvider(string? signUrl) : AndroidBotSignProvider, IDisposable
{
    private const string Tag = nameof(AndroidSignProvider);

    private readonly HttpClient _client = new();

    private readonly string _url = signUrl ?? "http://127.0.0.1:8081";

    [UnsafeAccessor(UnsafeAccessorKind.StaticField, Name = "WhiteListCommand")]
    private static extern ref HashSet<string> GetAndroidWhiteListCommand([UnsafeAccessorType("Lagrange.Core.Common.DefaultAndroidBotSignProvider, Lagrange.Core")] object? _);

    public override bool IsWhiteListCommand(string cmd) => GetAndroidWhiteListCommand(null).Contains(cmd);

    public override async Task<SsoSecureInfo?> GetSecSign(long uin, string cmd, int seq, ReadOnlyMemory<byte> body)
    {
        try
        {
            var payload = new JsonObject
            {
                ["uin"] = uin,
                ["cmd"] = cmd,
                ["seq"] = seq,
                ["buffer"] = Convert.ToHexString(body.Span),
                ["guid"] = Convert.ToHexString(Context.Keystore.Guid),
                ["version"] = Context.AppInfo.PtVersion
            };

            var response = await _client.PostAsync($"{_url}/sign", new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return null;

            var content = JsonHelper.Deserialize<ResponseRoot<SignResponse>>(await response.Content.ReadAsStringAsync());
            if (content == null) return null;

            return new SsoSecureInfo
            {
                SecSign = Convert.FromHexString(content.Value.Sign),
                SecToken = Convert.FromHexString(content.Value.Token),
                SecExtra = Convert.FromHexString(content.Value.Extra)
            };
        }
        catch (Exception e)
        {
            Context.LogWarning(Tag, "Failed to get sign: {0}", e, e.Message);
            return null;
        }
    }

    public override async Task<byte[]> GetEnergy(long uin, string data)
    {
        try
        {
            var payload = new JsonObject
            {
                ["uin"] = uin,
                ["data"] = data,
                ["guid"] = Convert.ToHexString(Context.Keystore.Guid),
                ["ver"] = Context.AppInfo.SdkInfo.SdkVersion,
                ["version"] = Context.AppInfo.PtVersion
            };

            var response = await _client.PostAsync($"{_url}/energy", new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return [];

            var content = JsonHelper.Deserialize<ResponseRoot<string>>(await response.Content.ReadAsStringAsync());
            return content == null ? [] : Convert.FromHexString(content.Value);
        }
        catch (Exception e)
        {
            Context.LogWarning(Tag, "Failed to get energy: {0}", e, e.Message);
            return [];
        }
    }

    public override async Task<byte[]> GetDebugXwid(long uin, string data)
    {
        try
        {
            var payload = new JsonObject
            {
                ["uin"] = uin,
                ["data"] = data,
                ["guid"] = Convert.ToHexString(Context.Keystore.Guid),
                ["version"] = Context.AppInfo.PtVersion
            };

            var response = await _client.PostAsync($"{_url}/get_tlv553", new StringContent(payload.ToJsonString(), Encoding.UTF8, "application/json"));
            if (!response.IsSuccessStatusCode) return [];

            var content = JsonHelper.Deserialize<ResponseRoot<string>>(await response.Content.ReadAsStringAsync());
            return content == null ? [] : Convert.FromHexString(content.Value);
        }
        catch (Exception e)
        {
            Context.LogWarning(Tag, "Failed to get debug_xwid: {0}", e, e.Message);
            return [];
        }
    }

    public void Dispose()
    {
        _client.Dispose();
    }

    [Serializable]
    internal class ResponseRoot<T>
    {
        [JsonPropertyName("data")] public T Value { get; set; } = default!;
    }

    [Serializable]
    internal class SignResponse
    {
        [JsonPropertyName("sign")] public string Sign { get; set; } = string.Empty;

        [JsonPropertyName("token")] public string Token { get; set; } = string.Empty;

        [JsonPropertyName("extra")] public string Extra { get; set; } = string.Empty;
    }
}

internal static partial class JsonHelper
{
    [JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Default, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]

    [JsonSerializable(typeof(AndroidSignProvider.ResponseRoot<AndroidSignProvider.SignResponse>))]
    [JsonSerializable(typeof(AndroidSignProvider.ResponseRoot<string>))]
    [JsonSerializable(typeof(LinuxSignProvider.Root))]
    [JsonSerializable(typeof(LinuxSignProvider.Response))]

    [JsonSerializable(typeof(JsonObject))]
    [JsonSerializable(typeof(LightApp))]
    private partial class CoreSerializerContext : JsonSerializerContext;

    public static T? Deserialize<T>(string json) where T : class =>
        JsonSerializer.Deserialize(json, typeof(T), CoreSerializerContext.Default) as T;

    public static string Serialize<T>(T value) =>
        JsonSerializer.Serialize(value, typeof(T), CoreSerializerContext.Default);

    public static ReadOnlyMemory<byte> SerializeToUtf8Bytes<T>(T value) =>
        JsonSerializer.SerializeToUtf8Bytes(value, typeof(T), CoreSerializerContext.Default);
}
