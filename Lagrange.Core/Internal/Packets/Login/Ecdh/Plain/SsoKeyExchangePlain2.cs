using Lagrange.Core.Utility.Binary;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Login.Ecdh.Plain;

/// <summary>
/// Sha256 Encrypt this packet to get content of GcmCalc2, Key is constant
/// </summary>
[Serializable]
internal class SsoKeyExchangePlain2
{
    [BinaryProperty(Prefix.None)] public byte[] PublicKey { get; set; }
    
    [BinaryProperty] public uint Type { get; set; }
    
    [BinaryProperty(Prefix.None)] public byte[] EncryptedGcm { get; set; }

    [BinaryProperty] public uint Const { get; set; } = 0;
    
    [BinaryProperty] public uint Timestamp { get; set; }
}