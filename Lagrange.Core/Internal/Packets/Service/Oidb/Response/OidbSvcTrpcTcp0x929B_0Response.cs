using Lagrange.Core.Internal.Packets.Service.Oidb.Common;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

#pragma warning disable CS8618
[ProtoContract]
internal class OidbSvcTrpcTcp0x929B_0Response
{
    [ProtoMember(1)] uint Field1 { get; set; } //1

    [ProtoMember(2)] uint Field2 { get; set; } //319

    [ProtoMember(3)] uint Field3 { get; set; } //20

    [ProtoMember(4)] public MsgInfo MsgInfo { get; set; }
}