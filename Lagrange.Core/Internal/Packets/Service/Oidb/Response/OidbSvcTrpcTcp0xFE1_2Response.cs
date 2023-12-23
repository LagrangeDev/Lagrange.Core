using ProtoBuf;

#pragma warning disable CS8618
// Resharper Disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;


[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2Response
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0xFE1_2ResponseBody Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xFE1_2ResponseBody
{
    [ProtoMember(1)] public string Uid { get; set; } = "";
    
    [ProtoMember(3)] public uint Uin { get; set; }
}