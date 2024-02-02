using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class VideoFile
{
    [ProtoMember(1)] public string FileUuid { get; set; }
    
    [ProtoMember(2)] public byte[] FileMd5 { get; set; }
    
    [ProtoMember(3)] public string FileName { get; set; }
    
    [ProtoMember(4)] public int FileFormat { get; set; }
    
    [ProtoMember(5)] public int FileTime { get; set; }
    
    [ProtoMember(6)] public int FileSize { get; set; }
    
    [ProtoMember(7)] public int ThumbWidth { get; set; }
    
    [ProtoMember(8)] public int ThumbHeight { get; set; }
    
    [ProtoMember(9)] public byte[] ThumbFileMd5 { get; set; }
    
    [ProtoMember(10)] public byte[] Source { get; set; }
    
    [ProtoMember(11)] public int ThumbFileSize { get; set; }
    
    [ProtoMember(12)] public int BusiType { get; set; }
    
    [ProtoMember(13)] public int FromChatType { get; set; }
    
    [ProtoMember(14)] public int ToChatType { get; set; }
    
    [ProtoMember(15)] public bool BoolSupportProgressive { get; set; }
    
    [ProtoMember(16)] public int FileWidth { get; set; }
    
    [ProtoMember(17)] public int FileHeight { get; set; }
    
    [ProtoMember(18)] public int SubBusiType { get; set; }
    
    [ProtoMember(19)] public int VideoAttr { get; set; }
    
    [ProtoMember(20)] public byte[][] BytesThumbFileUrls { get; set; }
    
    [ProtoMember(21)] public byte[][] BytesVideoFileUrls { get; set; }
    
    [ProtoMember(22)] public int ThumbDownloadFlag { get; set; }
    
    [ProtoMember(23)] public int VideoDownloadFlag { get; set; }
    
    [ProtoMember(24)] public byte[] PbReserve { get; set; }
}