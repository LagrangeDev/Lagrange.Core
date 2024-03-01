using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class CustomFace
{
    [ProtoMember(1)] public byte[] Guid { get; set; }
    
    [ProtoMember(2)] public string FilePath { get; set; }
    
    [ProtoMember(3)] public string Shortcut { get; set; }
    
    [ProtoMember(4)] public byte[] Buffer { get; set; }
    
    [ProtoMember(5)] public byte[] Flag { get; set; }
    
    [ProtoMember(6)] public byte[] OldData { get; set; }
    
    [ProtoMember(7)] public uint FileId { get; set; }
    
    [ProtoMember(8)] public int? ServerIp { get; set; }
    
    [ProtoMember(9)] public int? ServerPort { get; set; }
    
    [ProtoMember(10)] public int FileType { get; set; }
    
    [ProtoMember(11)] public byte[] Signature { get; set; }
    
    [ProtoMember(12)] public int Useful { get; set; }
    
    [ProtoMember(13)] public byte[] Md5 { get; set; }
    
    [ProtoMember(14)] public string ThumbUrl { get; set; }
    
    [ProtoMember(15)] public string BigUrl { get; set; }
    
    [ProtoMember(16)] public string OrigUrl { get; set; }
    
    [ProtoMember(17)] public int BizType { get; set; }
    
    [ProtoMember(18)] public int RepeatIndex { get; set; }
    
    [ProtoMember(19)] public int RepeatImage { get; set; }
    
    [ProtoMember(20)] public int ImageType { get; set; }
    
    [ProtoMember(21)] public int Index { get; set; }
    
    [ProtoMember(22)] public int Width { get; set; }
    
    [ProtoMember(23)] public int Height { get; set; }
    
    [ProtoMember(24)] public int Source { get; set; }
    
    [ProtoMember(25)] public uint Size { get; set; }
    
    [ProtoMember(26)] public int Origin { get; set; }
    
    [ProtoMember(27)] public int? ThumbWidth { get; set; }
    
    [ProtoMember(28)] public int? ThumbHeight { get; set; }
    
    [ProtoMember(29)] public int ShowLen { get; set; }
    
    [ProtoMember(30)] public int DownloadLen { get; set; }
    
    [ProtoMember(31)] public string? X400Url { get; set; }
    
    [ProtoMember(32)] public int X400Width { get; set; }
    
    [ProtoMember(33)] public int X400Height { get; set; }
    
    [ProtoMember(34)] public byte[]? PbReserve { get; set; }
}