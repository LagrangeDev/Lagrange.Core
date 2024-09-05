using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Group Member Mute
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xBCB, 0)]
internal class OidbSvcTrpcTcp0xBCB_0
{
    [ProtoMember(10)] public OidbSvcTrpcTcp0xBCB_0Body Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xBCB_0Body
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xBCB_0URL Body { get; set; }

    [ProtoMember(2)] public uint Expires { get; set; } = 300;
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xBCB_0URL
{
    [ProtoMember(1)] public string Url { get; set; }

    [ProtoMember(2)] public uint Refer { get; set; } = 0;

    [ProtoMember(3)] public uint Plateform { get; set; } = 0;

    [ProtoMember(5)] public uint Type { get; set; } = 15;

    [ProtoMember(6)] public uint Form { get; set; } = 0;

    [ProtoMember(7)] public uint Chatid { get; set; } = 0;

    [ProtoMember(8)] public uint ServiceType { get; set; } = 1;

    [ProtoMember(9)] public uint SendUin { get; set; } = 0;

}