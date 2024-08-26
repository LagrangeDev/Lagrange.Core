using Lagrange.Core.Internal.Packets.Login.Ecdh.Plain;
using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Login.NTLogin;

[ProtoContract]
internal class SsoNTLoginEncryptedData
{
    /// <summary>From <see cref="SsoKeyExchangeDecrypted"/> Sign field, just simply store that in keystore</summary>
    [ProtoMember(1)] public byte[]? Sign { get; set; }

    [ProtoMember(3)] public byte[]? GcmCalc { get; set; }

    [ProtoMember(4)] public int Type { get; set; }
}