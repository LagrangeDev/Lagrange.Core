using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Core.Packets.Service.Oidb.Internal;

[ProtoContract]
[OidbSvcTrpcTcp(0xCDE, 2, true)]
internal class OidbSvcTrpcTcp0xCDE_2
{
    [ProtoMember(2)] public OidbSvcTrpcTcp0xCDE_2Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xCDE_2Body
{
    [ProtoMember(1)] public string Field1 { get; set; }
}