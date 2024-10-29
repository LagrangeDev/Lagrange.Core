using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
[ProtoContract]
[OidbSvcTrpcTcp(0x929B, 0)]
internal class OidbSvcTrpcTcp0x929B_0
{
    [ProtoMember(1)] public uint Group { get; set; }

    [ProtoMember(2)] public string tts { get; set; }

    [ProtoMember(3)] public string Msg { get; set; }

    [ProtoMember(4)] private uint Field4 { get; set; } = 1;

    [ProtoMember(5)] OidbSvcTrpcTcp0x929B_0Random Field5 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x929B_0Random
{
    [ProtoMember(1)] private uint Random { get; set; } = 233;
}