using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

[ProtoContract]
[OidbSvcTrpcTcp(0xE07, 0)]
internal class OidbSvcTrpcTcp0xE07_0
{
    [ProtoMember(1)] public uint Version { get; set; } 
    
    [ProtoMember(2)] public uint Client { get; set; }
    
    [ProtoMember(3)] public uint Entrance { get; set; }
    
    [ProtoMember(10)] public OcrReqBody OcrReqBody { get; set; }
}

[ProtoContract]
internal class OcrReqBody
{
    [ProtoMember(1)] public string ImageUrl { get; set; } 
    
    [ProtoMember(2)] public uint LanguageType { get; set; }
    
    [ProtoMember(3)] public uint Scene { get; set; }
    
    [ProtoMember(10)] public string OriginMd5 { get; set; }
    
    [ProtoMember(11)] public string AfterCompressMd5 { get; set; }
    
    [ProtoMember(12)] public string AfterCompressFileSize { get; set; }
    
    [ProtoMember(13)] public string AfterCompressWeight { get; set; }
    
    [ProtoMember(14)] public string AfterCompressHeight { get; set; }
    
    [ProtoMember(15)] public bool IsCut { get; set; }
}