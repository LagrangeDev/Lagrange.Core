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
        public uint[]? SubAppIdList { get; internal set; }
        public byte[] PubKey { get; internal set; }
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
            MiscBitmap = 32764,
            SubSigBitmap = 0,
            PubKey = new byte[]
            {
                0x04, 0x92, 0x8D, 0x88, 0x50, 0x67, 0x30, 0x88,
                0xB3, 0x43, 0x26, 0x4E, 0x0C, 0x6B, 0xAC, 0xB8,
                0x49, 0x6D, 0x69, 0x77, 0x99, 0xF3, 0x72, 0x11,
                0xDE, 0xB2, 0x5B, 0xB7, 0x39, 0x06, 0xCB, 0x08,
                0x9F, 0xEA, 0x96, 0x39, 0xB4, 0xE0, 0x26, 0x04,
                0x98, 0xB5, 0x1A, 0x99, 0x2D, 0x50, 0x81, 0x3D,
                0xA8
            }
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
            MiscBitmap = 32764,
            SubSigBitmap = 0,
            PubKey = new byte[]
            {
                0x04, 0x92, 0x8D, 0x88, 0x50, 0x67, 0x30, 0x88,
                0xB3, 0x43, 0x26, 0x4E, 0x0C, 0x6B, 0xAC, 0xB8,
                0x49, 0x6D, 0x69, 0x77, 0x99, 0xF3, 0x72, 0x11,
                0xDE, 0xB2, 0x5B, 0xB7, 0x39, 0x06, 0xCB, 0x08,
                0x9F, 0xEA, 0x96, 0x39, 0xB4, 0xE0, 0x26, 0x04,
                0x98, 0xB5, 0x1A, 0x99, 0x2D, 0x50, 0x81, 0x3D,
                0xA8
            }
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
            MiscBitmap = 32764,
            SubSigBitmap = 0,
            PubKey = new byte[]
            {
                0x04, 0x92, 0x8D, 0x88, 0x50, 0x67, 0x30, 0x88,
                0xB3, 0x43, 0x26, 0x4E, 0x0C, 0x6B, 0xAC, 0xB8,
                0x49, 0x6D, 0x69, 0x77, 0x99, 0xF3, 0x72, 0x11,
                0xDE, 0xB2, 0x5B, 0xB7, 0x39, 0x06, 0xCB, 0x08,
                0x9F, 0xEA, 0x96, 0x39, 0xB4, 0xE0, 0x26, 0x04,
                0x98, 0xB5, 0x1A, 0x99, 0x2D, 0x50, 0x81, 0x3D,
                0xA8
            }
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
            SubAppIdList = new uint[] { 1600000226 },
            PubKey = new byte[]
            {
                0x04,
                0xEB, 0xCA, 0x94, 0xD7, 0x33, 0xE3, 0x99, 0xB2,
                0xDB, 0x96, 0xEA, 0xCD, 0xD3, 0xF6, 0x9A, 0x8B,
                0xB0, 0xF7, 0x42, 0x24, 0xE2, 0xB4, 0x4E, 0x33,
                0x57, 0x81, 0x22, 0x11, 0xD2, 0xE6, 0x2E, 0xFB,
                0xC9, 0x1B, 0xB5, 0x53, 0x09, 0x8E, 0x25, 0xE3,
                0x3A, 0x79, 0x9A, 0xDC, 0x7F, 0x76, 0xFE, 0xB2,
                0x08, 0xDA, 0x7C, 0x65, 0x22, 0xCD, 0xB0, 0x71,
                0x9A, 0x30, 0x51, 0x80, 0xCC, 0x54, 0xA8, 0x2E
            }
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
            SubAppIdList = new uint[] { 1600000226 },
            PubKey = new byte[]
            {
                0x04,
                0xEB, 0xCA, 0x94, 0xD7, 0x33, 0xE3, 0x99, 0xB2,
                0xDB, 0x96, 0xEA, 0xCD, 0xD3, 0xF6, 0x9A, 0x8B,
                0xB0, 0xF7, 0x42, 0x24, 0xE2, 0xB4, 0x4E, 0x33,
                0x57, 0x81, 0x22, 0x11, 0xD2, 0xE6, 0x2E, 0xFB,
                0xC9, 0x1B, 0xB5, 0x53, 0x09, 0x8E, 0x25, 0xE3,
                0x3A, 0x79, 0x9A, 0xDC, 0x7F, 0x76, 0xFE, 0xB2,
                0x08, 0xDA, 0x7C, 0x65, 0x22, 0xCD, 0xB0, 0x71,
                0x9A, 0x30, 0x51, 0x80, 0xCC, 0x54, 0xA8, 0x2E
            }
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