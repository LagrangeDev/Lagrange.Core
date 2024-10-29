using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;
#pragma warning disable CS8618
[ProtoContract]
[OidbSvcTrpcTcp(0x929B, 0)]
internal class OidbSvcTrpcTcp0x929C_0
{
    [ProtoMember(1)] public uint Group { get; set; }

    [ProtoMember(2)] public uint f2 { get; set; } = 1;
}