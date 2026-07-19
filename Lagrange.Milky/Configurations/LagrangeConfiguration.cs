using Lagrange.Core.Common;

namespace Lagrange.Milky.Configurations;

public class LagrangeConfiguration(LagrangeProtocolConfiguration protocol, LagrangeLoginConfiguration login, LagrangeServerConfiguration? server = null, LagrangeQrCodeConfiguration? qrCode = null)
{
    public LagrangeProtocolConfiguration Protocol { get; } = protocol;
    public LagrangeLoginConfiguration Login { get; } = login;
    public LagrangeServerConfiguration Server { get; } = server ?? new();
    public LagrangeQrCodeConfiguration QrCode { get; } = qrCode ?? new();
}

public class LagrangeQrCodeConfiguration(bool compatible = false)
{
    public bool Compatible { get; } = compatible;
}

public class LagrangeLoginConfiguration(long uin, string? password = null, bool autoReLogin = false, bool useOnlineCaptchResolver = true)
{
    public long Uin { get; } = uin;
    public string? Password { get; } = password;
    public bool AutoReLogin { get; } = autoReLogin;
    public bool UseOnlineCaptchResolver { get; } = useOnlineCaptchResolver;
}

public class LagrangeServerConfiguration(bool autoReconnect = true, bool useIPv6Network = false, bool getOptimumServer = true)
{
    public bool AutoReconnect { get; } = autoReconnect;
    public bool UseIPv6Network { get; } = useIPv6Network;
    public bool GetOptimumServer { get; } = getOptimumServer;
}

public class LagrangeProtocolConfiguration(LagrangeSignerConfiguration signer, Platform platform = Platform.Linux, BotAppInfo? appInfo = null)
{
    public Platform Platform { get; } = platform;
    public BotAppInfo? AppInfo { get; } = appInfo;

    public LagrangeSignerConfiguration Signer { get; } = signer;
}

public class LagrangeSignerConfiguration(string baseUrl, string token, string? proxyUrl = null)
{
    public string BaseUrl { get; } = baseUrl;
    public string NormalizedBaseUrl => BaseUrl.EndsWith('/') ? BaseUrl : $"{BaseUrl}/";
    public string Token { get; } = token;

    public string? ProxyUrl { get; } = proxyUrl;
}

public enum Platform
{
    Windows = 0b00000001,
    Linux = 0b00000100,
    MacOS = 0b00000010,
}