using ProtoBuf;

// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x773, 0, true)]
internal class OidbSvcTrpcTcp0x773_0
{
    [ProtoMember(1)] public int Field1 { get; set; }
}