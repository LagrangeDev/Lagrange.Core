using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Login.Ecdh.Plain;

[ProtoContract]
internal class SsoKeyExchangeDecrypted
{
    [ProtoMember(1)] public byte[] GcmKey { get; set; }

    [ProtoMember(2)] public byte[] Sign { get; set; }

    [ProtoMember(3)] public uint Expiration { get; set; }
}