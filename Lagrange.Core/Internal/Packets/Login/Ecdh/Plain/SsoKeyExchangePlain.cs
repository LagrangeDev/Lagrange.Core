using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.Ecdh.Plain;

[ProtoContract]
internal class SsoKeyExchangePlain
{
    [ProtoMember(1)] public string? Uin { get; set; }

    [ProtoMember(2)] public byte[]? Guid { get; set; }
}