using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1200Response
{
    [ProtoMember(1)] public uint Command { get; set; }
    
    [ProtoMember(2)] public uint SubCommand { get; set; }
    
    [ProtoMember(14)] public OidbSvcTrpcTcp0xE37_1200ResponseBody Body { get; set; }
    
    [ProtoMember(50)] public uint Field50 { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1200ResponseBody
{
    [ProtoMember(10)] public uint Field10 { get; set; }

    [ProtoMember(20)] public string State { get; set; }
    
    [ProtoMember(30)] public OidbSvcTrpcTcp0xE37_1200Result Result { get; set; }
    
    [ProtoMember(40)] public OidbSvcTrpcTcp0xE37_1200Metadata Metadata { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1200Result
{
    [ProtoMember(20)] public string Server { get; set; }
    
    [ProtoMember(40)] public uint Port { get; set; }
    
    [ProtoMember(50)] public string Url { get; set; }
    
    [ProtoMember(60)] public List<string> AdditionalServer { get; set; }
    
    [ProtoMember(80)] public uint SsoPort { get; set; }
    
    [ProtoMember(90)] public string SsoUrl { get; set; }
    
    [ProtoMember(120)] public byte[] Extra { get; set; }
}

[ProtoContract]
internal class OidbSvcTrpcTcp0xE37_1200Metadata
{
    [ProtoMember(1)] public uint Uin { get; set; }
    
    [ProtoMember(2)] public uint Field2 { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; }
    
    [ProtoMember(4)] public uint Size { get; set; }
    
    [ProtoMember(5)] public uint Timestamp { get; set; }
    
    [ProtoMember(6)] public string FileUuid { get; set; }
    
    [ProtoMember(7)] public string FileName { get; set; }
    
    [ProtoMember(100)] public byte[] Field100 { get; set; }
    
    [ProtoMember(101)] public byte[] Field101 { get; set; }
    
    [ProtoMember(110)] public uint Field110 { get; set; }
    
    [ProtoMember(130)] public uint Timestamp1 { get; set; }
    
    [ProtoMember(140)] public string FileHash { get; set; }
    
    [ProtoMember(141)] public byte[] Field141 { get; set; } // identical to Field100
    
    [ProtoMember(142)] public byte[] Field142 { get; set; } // identical to Field101
}
