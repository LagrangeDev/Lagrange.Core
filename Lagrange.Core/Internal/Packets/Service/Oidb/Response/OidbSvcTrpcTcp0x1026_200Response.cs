using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0x1026_200Response
{
    [ProtoMember(3)] public OidbSvcTrpcTcp0x1026_100ResponseBody Body { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x1026_100ResponseBody
{
    [ProtoMember(1)] public string DownloadParams { get; set; }
    
    [ProtoMember(2)] public uint ExpireTime { get; set; }

    [ProtoMember(3)] public OidbSvcTrpcTcp0x1026_100ResponseBodyField3 Field3 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0x1026_100ResponseBodyField3
{
    [ProtoMember(1)] public string Domain { get; set; }
    
    [ProtoMember(2)] public string Suffix { get; set; }
    
    [ProtoMember(3)] public uint Port { get; set; }
    
    // [ProtoMember(4)]
    
    // [ProtoMember(5)]
}