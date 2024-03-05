#pragma warning disable CS8618

using Lagrange.Core.Utility.Sign;

namespace Lagrange.Core.Common;

public class BotAppInfo
{
    public class WtLoginSdkDef
    {
        public uint SdkBuildTime { get; internal set; }
        public string SdkVersion { get; internal set; }
        public uint MainSigBitmap { get; internal set; }
        public uint MiscBitmap { get; internal set; }
        public uint SubSigBitmap { get; internal set; }
        public uint[] SubAppIdList { get; internal set; }
    }

    public string Os { get; private set; }

    public string VendorOs { get; private set; }

    public string Kernel { get; private set; }

    public string BaseVersion { get; private set; }

    public string CurrentVersion { get; private set; }

    public int BuildVersion { get; private set; }

    public WtLoginSdkDef WtLoginSdk { get; private set; }

    public string PtVersion { get; private set; }

    public int PtOsVersion { get; private set; }

    public string PackageName { get; private set; }


    /// <summary>Or Known as QUA</summary>
    public string PackageSign { get; private set; }

    public uint AppId { get; private set; }

    /// <summary>Or known as pubId in tencent log</summary>
    public uint SubAppId { get; private set; }

    public uint AppIdQrCode { get; private set; }

    public ushort AppClientVersion { get; private set; }

    public ushort NTLoginType { get; private set; }

    public byte[] ApkSignatureMd5 { get; private set; }

    public string AppKey { get; private set; }

    public SignProvider SignProvider { get; internal set; }


    private static readonly BotAppInfo Linux = new()
    {
        Os = "Linux",
        Kernel = "Linux",
        VendorOs = "linux",

        BaseVersion = "3.1.1-11223",
        CurrentVersion = "3.1.2-13107",
        BuildVersion = 13107,
        PtVersion = "2.0.0",
        PtOsVersion = 19,
        PackageName = "com.tencent.qq",
        PackageSign = "V1_LNX_NQ_3.1.2-13107_RDM_B",
        AppId = 1600001615,
        SubAppId = 537146866,
        AppIdQrCode = 13697054,
        AppClientVersion = 13172,
        WtLoginSdk = new()
        {
            SdkVersion = "nt.wtlogin.0.0.1",
            MainSigBitmap = 169742560,
            MiscBitmap = 12058620,
            SubSigBitmap = 0,
        },

        NTLoginType = 1,

        SignProvider = new LinuxSigner()
    };

    private static readonly BotAppInfo MacOs = new()
    {
        Os = "Mac",
        Kernel = "Darwin",
        VendorOs = "mac",

        BaseVersion = "6.9.17-12118",
        CurrentVersion = "6.9.23-20139",
        BuildVersion = 20139,
        PtVersion = "2.0.0",
        PtOsVersion = 23,
        PackageName = "com.tencent.qq",
        PackageSign = "V1_MAC_NQ_6.9.23-20139_RDM_B",
        AppId = 1600001602,
        SubAppId = 537200848,
        AppIdQrCode = 537200848,
        AppClientVersion = 13172,
        WtLoginSdk = new()
        {
            SdkBuildTime = 1702888273,
            SdkVersion = "nt.wtlogin.0.0.1",
            MainSigBitmap = 169742560,
            MiscBitmap = 12058620,
            SubSigBitmap = 0,
        },

        NTLoginType = 5,

        SignProvider = new MacSigner()
    };

    private static readonly BotAppInfo Windows = new()
    {
        Os = "Windows",
        Kernel = "Windows_NT",
        VendorOs = "win32",

        BaseVersion = "9.9.1-15489",
        CurrentVersion = "",
        BuildVersion = 15962,
        PtVersion = "2.0.0",
        PtOsVersion = 23,
        PackageName = "com.tencent.qq",
        PackageSign = "V1_WIN_NQ_9.9.7_21159_GW_B",
        AppId = 1600001604,
        SubAppId = 537207105,
        AppIdQrCode = 537138217,
        AppClientVersion = 13172,
        WtLoginSdk = new()
        {
            SdkBuildTime = 1702888273,
            SdkVersion = "nt.wtlogin.0.0.1",
            MainSigBitmap = 169742560,
            MiscBitmap = 12058620,
            SubSigBitmap = 0,
        },

        NTLoginType = 5,

        SignProvider = new WindowsSigner()
    };

    private static readonly BotAppInfo AndroidPhone = new()
    {
        Os = "Android",
        Kernel = "",
        VendorOs = "",

        BaseVersion = "9.0.20.15515",
        CurrentVersion = "A9.0.20.38faf5bf",
        BuildVersion = 15962,
        PtVersion = "9.0.20",
        PtOsVersion = 23,
        PackageName = "com.tencent.mobileqq",
        PackageSign = "V1_AND_SQ_9.0.20_5844_YYB_D",
        AppId = 16,
        SubAppId = 537206436,
        AppKey = "0S200MNJT807V3GE",
        ApkSignatureMd5 = new byte[] { 0xA6, 0xB7, 0x45, 0xBF, 0x24, 0xA2, 0xC2, 0x77, 0x52, 0x77, 0x16, 0xF6, 0xF3, 0x6E, 0xB6, 0x8D },
        AppClientVersion = 0,
        WtLoginSdk = new()
        {
            SdkBuildTime = 1702888273,
            SdkVersion = "6.0.0.2558",
            MainSigBitmap = 34869472,
            MiscBitmap = 150470524,
            SubSigBitmap = 66560,
            SubAppIdList = new uint[] { 1600000226 }
        },

        SignProvider = new AndroidSigner()
    };

    private static readonly BotAppInfo AndroidPad = new()
    {
        Os = "Android",
        Kernel = "",
        VendorOs = "",

        BaseVersion = "9.0.20.15515",
        CurrentVersion = "A9.0.20.38faf5bf",
        BuildVersion = 15962,
        PtVersion = "9.0.20",
        PtOsVersion = 23,
        PackageName = "com.tencent.mobileqq",
        PackageSign = "V1_AND_SQ_9.0.20_5844_YYB_D",
        AppId = 16,
        SubAppId = 537206475,
        AppKey = "0S200MNJT807V3GE",
        ApkSignatureMd5 = new byte[] { 0xA6, 0xB7, 0x45, 0xBF, 0x24, 0xA2, 0xC2, 0x77, 0x52, 0x77, 0x16, 0xF6, 0xF3, 0x6E, 0xB6, 0x8D },
        AppClientVersion = 0,
        WtLoginSdk = new()
        {
            SdkBuildTime = 1702888273,
            SdkVersion = "6.0.0.2558",
            MainSigBitmap = 34869472,
            MiscBitmap = 150470524,
            SubSigBitmap = 66560,
            SubAppIdList = new uint[] { 1600000226 }
        },

        SignProvider = new AndroidSigner()
    };

    public static readonly Dictionary<Protocols, BotAppInfo> ProtocolToAppInfo = new()
    {
        { Protocols.Windows, Windows },
        { Protocols.Linux, Linux },
        { Protocols.MacOs, MacOs },
        { Protocols.AndroidPhone, AndroidPhone },
        { Protocols.AndroidPad, AndroidPad }
    };
}