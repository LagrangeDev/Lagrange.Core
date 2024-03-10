using Lagrange.Core.Common;
using Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Body;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Tlv;
using Lagrange.Core.Utility.Binary.Tlv.Attributes;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Tlv;

[Tlv(0x106)]
[TlvEncrypt(TlvEncryptAttribute.KeyType.PasswordWithSalt)]
internal class Tlv106 : TlvBody
{
    /// <summary>
    /// <para>manually construct Tlv106 by tempPassword, from TlvQrCode18, not through dependency injection</para>
    /// <para>This field does not only use as the request, but also response</para>
    /// <para>Response should be referred to <see cref="SsoNTLoginEasyLogin"/></para>

    /// <para>Password is depreciated, so such field is now injected through dependency injection</para>
    /// </summary>
    public Tlv106(BotKeystore keystore, BotAppInfo appInfo, BotDeviceInfo device)
    {
        Rand = (uint)Random.Shared.Next();
        AppId= appInfo.AppId;
        Uin = keystore.Uin;
        Time = (uint)DateTimeOffset.Now.ToUnixTimeSeconds();
        PasswordMd5 = keystore.PasswordMd5;
        TgtgtKey = keystore.Stub.TgtgtKey;
        Guid = device.System.Guid.ToByteArray();
        SubAppId = appInfo.SubAppId;
        UinStr = keystore.Uin.ToString();
    }

    [BinaryProperty] public ushort TGTGTVer { get; set; } = 4;
    [BinaryProperty] public uint Rand { get; set; }
    [BinaryProperty] public uint SSoVer { get; set; } = 21;
    [BinaryProperty] public uint AppId { get; set; }
    [BinaryProperty] public uint u1 { get; set; } = 0;
    [BinaryProperty] public ulong Uin { get; set; }
    [BinaryProperty] public uint Time { get; set; }
    [BinaryProperty] public uint IP { get; set; } = 0;
    [BinaryProperty] public byte u2 { get; set; } = 1;
    [BinaryProperty] public byte[] PasswordMd5 { get; set; }
    [BinaryProperty] public byte[] TgtgtKey { get; set; }
    [BinaryProperty] public uint u3 { get; set; } = 0;
    [BinaryProperty] public byte u4 { get; set; } = 1;
    [BinaryProperty] public byte[] Guid { get; set; }
    [BinaryProperty] public uint SubAppId { get; set; }
    [BinaryProperty] public uint u5 { get; set; } = 1;
    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string UinStr { get; set; }
    [BinaryProperty] public ushort u6 { get; set; } = 0;
}

[Tlv(0x106)]
internal class Tlv106V2 : TlvBody
{
    public Tlv106V2(BotKeystore keystore) => TempPassword = keystore.Session.TempPassword ?? Array.Empty<byte>();

    [BinaryProperty(Prefix.None)] public byte[] TempPassword { get; set; }
}

[Tlv(0x106, true)]
internal class Tlv106Response : TlvBody
{
    [BinaryProperty(Prefix.None)] public byte[] TempPassword { get; set; }
}