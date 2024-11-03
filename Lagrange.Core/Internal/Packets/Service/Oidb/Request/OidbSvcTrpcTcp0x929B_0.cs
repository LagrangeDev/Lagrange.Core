using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
[ProtoContract]
[OidbSvcTrpcTcp(0x929B, 0)]
internal class OidbSvcTrpcTcp0x929B_0
{
    [ProtoMember(1)] public uint GroupCode { get; set; }

    [ProtoMember(2)] public string VoiceId { get; set; }

    [ProtoMember(3)] public string Text { get; set; }

    [ProtoMember(4)] public uint ChatType { get; set; } = 1; //1 voice,2 song

    [ProtoMember(5)] public OidbSvcTrpcTcp0x929B_0ClientMsgInfo ClientMsgInfo { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929B_0ClientMsgInfo
{
    [ProtoMember(1)] public uint MsgRandom { get; set; }
}