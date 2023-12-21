using ProtoBuf;

// Resharper Disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2Response
{
    [ProtoMember(3)] public uint Uin { get; set; }
}