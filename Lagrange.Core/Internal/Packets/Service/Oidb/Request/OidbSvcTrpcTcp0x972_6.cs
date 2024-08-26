using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
[OidbSvcTrpcTcp(0x972, 6)]
internal class OidbSvcTrpcTcp0x972_6
{
    [ProtoMember(1)] public string TargetUin { get; set; }

    [ProtoMember(3)] public OidbSvcTrpcTcp0x972_6Settings Settings { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x972_6Settings
{
    [ProtoMember(4)] public uint Field4 { get; set; } // 25

    [ProtoMember(11)] public string Field11 { get; set; } // ""

    [ProtoMember(55)] public string Setting { get; set; } // {"search_by_uid":true, "scenario":"related_people_and_groups_panel"}
}