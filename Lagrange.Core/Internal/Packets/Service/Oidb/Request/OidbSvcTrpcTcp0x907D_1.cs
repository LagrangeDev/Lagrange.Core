using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x907D, 1)]
internal class OidbSvcTrpcTcp0x907D_1
{
    [ProtoMember(1)] public uint BotId { get; set; }
    
    [ProtoMember(2)] public uint type { get; set; } = 2;
    
    [ProtoMember(3)] public uint On { get; set; } = 1;

    [ProtoMember(4)] public uint GroupUin { get; set; }
}