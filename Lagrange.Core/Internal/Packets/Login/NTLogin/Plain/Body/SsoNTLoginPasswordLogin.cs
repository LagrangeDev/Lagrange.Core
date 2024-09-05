using Lagrange.Core.Utility.Binary;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Body;

#pragma warning disable CS8618
// ReSharper disable once InconsistentNaming

/// <summary>
/// Should be serialized with <see cref="BinarySerializer"/> and encrypted with Tea, key to be investigated
/// </summary>
[Serializable]
internal class SsoNTLoginPasswordLogin
{
    [BinaryProperty] public ushort Const4 { get; set; } = 4;

    [BinaryProperty] public uint Random { get; set; }

    [BinaryProperty] public uint UnknownConst2 { get; set; } = 0;
    
    [BinaryProperty] public uint AppId { get; set; }
    
    [BinaryProperty] public uint Ver { get; set; } = 8001;
    
    [BinaryProperty] public uint UnknownConst3 { get; set; } = 0;

    [BinaryProperty] public uint Uin { get; set; }

    [BinaryProperty] public uint Timestamp { get; set; }

    [BinaryProperty] public uint UnknownConst4 { get; set; } = 0;

    [BinaryProperty] public byte Flag { get; set; } = 1;

    [BinaryProperty(Prefix.None)] public byte[] PasswordMd5 { get; set; }

    [BinaryProperty(Prefix.None)] public byte[] RandomBytes { get; set; }

    [BinaryProperty] public uint UnknownConst5 { get; set; } = 0;

    [BinaryProperty] public byte Flag2 { get; set; } = 1;

    [BinaryProperty(Prefix.None)] public byte[] Guid { get; set; }

    [BinaryProperty] public uint UnknownConst6 { get; set; } = 1;

    [BinaryProperty] public uint UnknownConst7 { get; set; } = 1;

    [BinaryProperty(Prefix.Uint16 | Prefix.LengthOnly)] public string UinString { get; set; }
}