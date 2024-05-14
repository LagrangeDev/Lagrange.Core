using System.Text;
using System.Text.Json.Serialization;
using Lagrange.Core.Utility.Crypto;
using Lagrange.Core.Utility.Extension;
using Lagrange.Core.Utility.Generator;


namespace Lagrange.Core.Common;

public class BotKeystore
{
    [JsonConstructor]
    public BotKeystore()
    {
        PasswordMd5 = "";
        
        SecpImpl = new EcdhImpl(EcdhImpl.CryptMethod.Secp192K1);
        PrimeImpl = new EcdhImpl(EcdhImpl.CryptMethod.Prime256V1, false);
        TeaImpl = new TeaImpl();

        Stub = new KeyCollection();
        
        var tempPwd = Session?.TempPassword;
        Session = tempPwd != null ? new WtLoginSession { TempPassword = tempPwd } : new WtLoginSession();
    }

    /// <summary>
    /// Create the Bot keystore
    /// </summary>
    /// <param name="uin">Set this field 0 to use QrCode Login</param>
    /// <param name="password">Password Raw</param>
    internal BotKeystore(uint uin, string password)
    {
        Uin = uin;
        PasswordMd5 = Encoding.UTF8.GetBytes(password).Md5();
        
        SecpImpl = new EcdhImpl(EcdhImpl.CryptMethod.Secp192K1);
        PrimeImpl = new EcdhImpl(EcdhImpl.CryptMethod.Prime256V1, false);
        TeaImpl = new TeaImpl();
        Session = new WtLoginSession();
        
        Stub = new KeyCollection();
    }
    
    public uint Uin { get; set; }
    
    public string? Uid { get; set; }

    public string PasswordMd5 { get; set; }

    internal EcdhImpl SecpImpl { get; set; }
    internal EcdhImpl PrimeImpl { get; set; }
    internal TeaImpl TeaImpl { get; set; }

    internal KeyCollection Stub { get; }
    
    public WtLoginSession Session { get; set; }
    
    public BotInfo? Info { get; set; }
    
    [Serializable]
    public class KeyCollection
    {
        public byte[] RandomKey { get; set; } = ByteGen.GenRandomBytes(16);
        public byte[] TgtgtKey { get; set; } = new byte[16];
    }
    
    [Serializable]
    public class WtLoginSession
    {
        public byte[] D2Key { get; set; } = new byte[16];
        public byte[] D2 { get; set; } = Array.Empty<byte>();
        public byte[] Tgt { get; set; } = Array.Empty<byte>();
        
        public DateTime SessionDate { get; set; }

        internal byte[]? QrSign { get; set; } // size: 24
        internal string? QrString { get; set; }
        internal string? QrUrl { get; set; }
        
        internal byte[]? ExchangeKey { get; set; }
        internal byte[]? KeySign { get; set; }
        internal byte[]? UnusualSign { get; set; }
        internal string? UnusualCookies { get; set; }
        internal string? CaptchaUrl { get; set; }
        internal string? NewDeviceVerifyUrl { get; set; }
        internal (string, string, string)? Captcha { get; set; }
        
        public byte[]? TempPassword { get; set; }
        internal byte[]? NoPicSig { get; set; } // size: 16, may be from Tlv19, for Tlv16A

        private ushort _sequence;
        internal ushort Sequence
        {
            get => _sequence++;
            set => _sequence = value;
        }
    }

    [Serializable]
    public class BotInfo
    {
        internal BotInfo(byte age, byte gender, string name)
        {
            Age = age;
            Gender = gender;
            Name = name;
        }

        [JsonConstructor]
        public BotInfo()
        {
            Name = "";
        }

        public byte Age { get; set; }
        
        public byte Gender { get; set; }
        
        public string Name { get; set; }

        public override string ToString() => $"Bot name: {Name} | Gender: {Gender} | Age: {Age}";
    }

    internal void ClearSession()
    {
        Session.D2 = Array.Empty<byte>();
        Session.Tgt = Array.Empty<byte>(); 
        Session.D2Key = new byte[16];
    }
}