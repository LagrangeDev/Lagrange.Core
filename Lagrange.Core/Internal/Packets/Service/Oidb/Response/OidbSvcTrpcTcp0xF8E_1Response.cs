using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xF8E_1Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xF8E_1ResponseBody Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xF8E_1ResponseBody
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public uint Sequence { get; set; }

    [ProtoMember(6)] public string Preview { get; set; }
}
