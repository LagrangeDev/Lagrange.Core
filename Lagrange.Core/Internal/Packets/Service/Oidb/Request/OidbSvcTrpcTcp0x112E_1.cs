using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x112E, 1)]
internal class OidbSvcTrpcTcp0x112E_1
{
    [ProtoMember(3)] public uint BotId { get; set; }
    
    [ProtoMember(4)] public uint Seq { get; set; } = 111111;
    
    [ProtoMember(5)] public string B_id { get; set; } = "";

    [ProtoMember(6)] public string B_data { get; set; } = "";

    [ProtoMember(7)] public uint IDD { get; set; } = 0;

    [ProtoMember(8)] public uint GroupUin { get; set; }

    [ProtoMember(9)] public uint GroupType { get; set; } = 1;
}