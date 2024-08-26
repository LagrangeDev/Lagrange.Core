using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x89A_0Response
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public string? ErorMsg { get; set; }
}