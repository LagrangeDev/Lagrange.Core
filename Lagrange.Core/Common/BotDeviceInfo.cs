using Lagrange.Core.Utility.Generator;

#pragma warning disable CS8618

namespace Lagrange.Core.Common;

[Serializable]
public class BotDeviceInfo
{
    public class ModelInfo
    {
        public string DeviceName { get; set; }

        public string Brand { get; set; }

        public string Imei { get; set; }

        public string Imsi { get; set; }

        public string BaseBand { get; set; }

        public string CodeName { get; set; }
    }

    public class SystemInfo
    {
        public string OsType { get; set; }

        public string OsVersion { get; set; }

        public string Version { get; set; }

        public string BootId { get; set; }

        public string BootLoader { get; set; }

        public string AndroidId { get; set; } = "";

        public string Incremental { get; set; }

        public string InnerVer { get; set; }

        public string FingerPrint { get; set; }

        public Guid Guid { get; set; }
    }

    public class NetworkInfo
    {
        public NetworkType NetworkType { get; set; }


        public string NetworkName { get; set; }

        public string Apn { get; set; }

        [Obsolete] public byte[] NetIpAddress { get; set; }

        public byte[] MacAddress { get; set; }

        public string WifiSsid { get; set; }

        public byte[] WifiBssid { get; set; }
    }

    public class DisplayInfo
    {
        public int Width { get; set; }

        public int Height { get; set; }
    }

    public enum NetworkType
    {
        Other = 0,
        Mobile = 1,
        Wifi = 2,
    }

    public ModelInfo Model { get; set; }

    public DisplayInfo Display { get; set; }

    public SystemInfo System { get; set; }

    public NetworkInfo Network { get; set; }

    public static BotDeviceInfo GenerateInfo(Protocols protocol) => new()
    {
        Model = new()
        {
            DeviceName = $"Lagrange-{StringGen.GenerateHex(6).ToUpper()}",
            Brand = "Lagrange",
            BaseBand = "",
            CodeName = "LagrangeDev/lagrange/lgrange:14/ABC1.240301.001/V816.0.24.3.1.DEV:user/release-keys",
            Imei = "",
            Imsi = ""
        },

        Display = new()
        {
            Width = 1080,
            Height = 1920
        },

        Network = new()
        {
            NetworkType = NetworkType.Wifi,
            NetworkName = "China Telecom",
            WifiSsid = "<Hidden SSID>",
            Apn = "wifi"
        },

        System = protocol switch
        {
            Protocols.Linux => new()
            {
                OsType = "Ubuntu 22.04",
                OsVersion = "22.04",
                Guid = Guid.NewGuid()
            },
            Protocols.MacOs => new()
            {
                OsType = "MacOs",
                OsVersion = "",
                Guid = Guid.NewGuid()
            },
            Protocols.Windows => new()
            {
                OsType = "Windows 10.0.19042",
                OsVersion = "10.0.19042.0",
                Guid = Guid.NewGuid()
            },
            Protocols.AndroidPhone or Protocols.AndroidPad => new()
            {
                OsType = "android",
                OsVersion = "14",
                Version = "V816.0.24.2.20.DEV",
                BootId = "REL",
                BootLoader = "V816.0.24.2.20.DEV",
                AndroidId = StringGen.GenerateHex(16),
                Incremental = StringGen.GenerateHex(16),
                InnerVer = "",
                FingerPrint = Guid.NewGuid().ToString(),
                Guid = Guid.NewGuid()
            },
            _ => throw new NotImplementedException()
        },
    };
}