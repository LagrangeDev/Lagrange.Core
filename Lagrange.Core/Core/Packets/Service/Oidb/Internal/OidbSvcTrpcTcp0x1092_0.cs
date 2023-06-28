using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0x1092, 0)]
internal class OidbSvcTrpcTcp0x1092_0
{
    [ProtoMember(1)] public string Field1 { get; set; } // Unknown
}