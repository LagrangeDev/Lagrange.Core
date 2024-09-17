#pragma warning disable CS8618

namespace Lagrange.Core.Common;

public class BotAppInfo
{
    public string Os { get; private set; }
    
    public string VendorOs { get; private set; }
    
    public string Kernel { get; private set; }

    public string CurrentVersion { get; private set; }

    public int MiscBitmap { get; private set; }
    
    public string PtVersion { get; private set; }
    
    public int SsoVersion { get; private set; }
    
    public string PackageName { get; private set; }
    
    public string WtLoginSdk { get; private set; }

    public int AppId { get; private set; }
    
    /// <summary>Or known as pubId in tencent log</summary>
    public int SubAppId { get; private set; }
    
    public int AppIdQrCode { get; private set; }
    
    public ushort AppClientVersion { get; private set; }
    
    public uint MainSigMap { get; private set; }
    
    public ushort SubSigMap { get; private set; }
    
    public ushort NTLoginType { get; private set; }

    private static readonly BotAppInfo Linux = new()
    {
        Os = "Linux",
        Kernel = "Linux",
        VendorOs = "linux",
        CurrentVersion = "3.2.10-25765",
        MiscBitmap = 32764,
        PtVersion = "2.0.0",
        SsoVersion = 19,
        PackageName = "com.tencent.qq",
        WtLoginSdk = "nt.wtlogin.0.0.1",
        AppId = 1600001615,
        SubAppId = 537234773,
        AppIdQrCode = 13697054,
        AppClientVersion = 25765,
        
        MainSigMap = 169742560,
        SubSigMap = 0,
        NTLoginType = 1
    };
    
    private static readonly BotAppInfo MacOs = new()
    {
        Os = "Mac",
        Kernel = "Darwin",
        VendorOs = "mac",
        CurrentVersion = "6.9.23-20139",
        PtVersion = "2.0.0",
        MiscBitmap = 32764,
        SsoVersion = 23,
        PackageName = "com.tencent.qq",
        WtLoginSdk = "nt.wtlogin.0.0.1",
        AppId = 1600001602,
        SubAppId = 537200848,
        AppIdQrCode = 537200848,
        AppClientVersion = 13172,
        
        MainSigMap = 169742560,
        SubSigMap = 0,
        NTLoginType = 5
    };
    
    private static readonly BotAppInfo Windows = new()
    {
        Os = "Windows",
        Kernel = "Windows_NT",
        VendorOs = "win32",
        CurrentVersion = "9.9.2-15962",
        PtVersion = "2.0.0",
        MiscBitmap = 32764,
        SsoVersion = 23,
        PackageName = "com.tencent.qq",
        WtLoginSdk = "nt.wtlogin.0.0.1",
        AppId = 1600001604,
        SubAppId = 537138217,
        AppIdQrCode = 537138217,
        AppClientVersion = 13172,
        
        MainSigMap = 169742560,
        SubSigMap = 0,
        NTLoginType = 5
    };
    
    public static readonly Dictionary<Protocols, BotAppInfo> ProtocolToAppInfo = new()
    {
        { Protocols.Windows, Windows },
        { Protocols.Linux, Linux },
        { Protocols.MacOs, MacOs },
    };
}