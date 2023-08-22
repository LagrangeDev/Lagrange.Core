using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Response;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class OidbSvcTrpcTcp0x8A0_1Response
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public string? ErrorMsg { get; set; }
}