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

    [ProtoMember(26)] public string? X400Url { get; set; }

    [ProtoMember(27)] public uint X400Width { get; set; }

    [ProtoMember(28)] public uint X400Height { get; set; }
    
    [ProtoMember(29)] public PbReserve PbRes { get; set; }

    [ProtoContract]
    public class PbReserve
    {
        [ProtoMember(1)] public int SubType { get; set; }

        [ProtoMember(3)] public int Field3 { get; set; }

        [ProtoMember(4)] public int Field4 { get; set; }

        [ProtoMember(8)] public string Summary { get; set; }

        [ProtoMember(10)] public int Field10 { get; set; }

        [ProtoMember(20)] public PbReserve2 Field20 { get; set; }

        [ProtoMember(30)] public string Url { get; set; }

        [ProtoMember(31)] public string Md5Str { get; set; }
    }

    [ProtoContract]
    public class PbReserve2
    {
        [ProtoMember(1)] public int Field1 { get; set; }

        [ProtoMember(2)] public string Field2 { get; set; }

        [ProtoMember(3)] public int Field3 { get; set; }

        [ProtoMember(4)] public int Field4 { get; set; }

        [ProtoMember(5)] public int Field5 { get; set; }

        [ProtoMember(7)] public string Field7 { get; set; }
    }
}