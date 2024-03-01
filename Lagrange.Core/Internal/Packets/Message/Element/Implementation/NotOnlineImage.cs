using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal partial class NotOnlineImage
{
    [ProtoMember(1)] public string FilePath { get; set; }
    
    [ProtoMember(2)] public uint FileLen { get; set; }
    
    [ProtoMember(3)] public string DownloadPath { get; set; }
    
    [ProtoMember(4)] public byte[] OldVerSendFile { get; set; }
    
    [ProtoMember(5)] public int ImgType { get; set; }
    
    [ProtoMember(6)] public byte[] PreviewsImage { get; set; }
    
    [ProtoMember(7)] public byte[] PicMd5 { get; set; }
    
    [ProtoMember(8)] public uint PicHeight { get; set; }
    
    [ProtoMember(9)] public uint PicWidth { get; set; }
    
    [ProtoMember(10)] public string ResId { get; set; }
    
    [ProtoMember(11)] public byte[] Flag { get; set; }
    
    [ProtoMember(12)] public string ThumbUrl { get; set; }
    
    [ProtoMember(13)] public int Original { get; set; }
    
    [ProtoMember(14)] public string BigUrl { get; set; }
    
    [ProtoMember(15)] public string OrigUrl { get; set; }
    
    [ProtoMember(16)] public int BizType { get; set; }
    
    [ProtoMember(17)] public int Result { get; set; }
    
    [ProtoMember(18)] public int Index { get; set; }
    
    [ProtoMember(19)] public byte[] OpFaceBuf { get; set; }
    
    [ProtoMember(20)] public bool OldPicMd5 { get; set; }
    
    [ProtoMember(21)] public int ThumbWidth { get; set; }
    
    [ProtoMember(22)] public int ThumbHeight { get; set; }
    
    [ProtoMember(23)] public int FileId { get; set; }
    
    [ProtoMember(24)] public uint ShowLen { get; set; }
    
    [ProtoMember(25)] public uint DownloadLen { get; set; }
    
    [ProtoMember(29)] public byte[] PbRes { get; set; }
}