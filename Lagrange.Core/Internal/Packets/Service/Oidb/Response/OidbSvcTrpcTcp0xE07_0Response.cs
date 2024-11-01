using ProtoBuf;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Response;

[ProtoContract]
internal class OidbSvcTrpcTcp0xE07_0_Response
{
    [ProtoMember(1)] public int RetCode { get; set; }
    
    [ProtoMember(2)] public string ErrMsg { get; set; }
    
    [ProtoMember(3)] public string Wording { get; set; }
    
    [ProtoMember(10)] public OcrRspBody OcrRspBody { get; set; }
}

[ProtoContract]
internal class OcrRspBody
{
    [ProtoMember(1)] public List<TextDetection> TextDetections { get; set; }
    
    [ProtoMember(2)] public string Language { get; set; }
    
    [ProtoMember(3)] public string RequestId { get; set; }
    
    [ProtoMember(101)] public List<string> OcrLanguageList { get; set; }
    
    [ProtoMember(102)] public List<string> DstTranslateLanguageList { get; set; }
    
    [ProtoMember(103)] public List<Language> LanguageList { get; set; }
    
    [ProtoMember(111)] public uint AfterCompressWeight { get; set; }
    
    [ProtoMember(112)] public uint AfterCompressHeight { get; set; }
}

[ProtoContract]
internal class TextDetection
{
    [ProtoMember(1)] public string DetectedText { get; set; }
    
    [ProtoMember(2)] public uint Confidence { get; set; }
    
    [ProtoMember(3)] public Polygon Polygon { get; set; }
    
    [ProtoMember(4)] public string AdvancedInfo { get; set; }
}

[ProtoContract]
internal class Polygon
{
    [ProtoMember(1)] public List<Coordinate> Coordinates { get; set; }
}

[ProtoContract]
internal class Coordinate
{
    [ProtoMember(1)] public int X { get; set; }
    
    [ProtoMember(2)] public int Y { get; set; }
}

[ProtoContract]
internal class Language
{
    [ProtoMember(1)] public string LanguageCode { get; set; }
    
    [ProtoMember(2)] public string LanguageDesc { get; set; }
}
