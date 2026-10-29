using Lagrange.Proto;

#pragma warning disable CS8618 // Non-nullable field is uninitialized.
namespace Lagrange.Core.Internal.Packets.Message;

[ProtoPackable]
internal partial class Elem
{
    [ProtoMember(1)] public Text? Text { get; set; } 
    
    [ProtoMember(4)] public NotOnlineImage? NotOnlineImage { get; set; }

    [ProtoMember(5)] public TransElem? TransElemInfo { get; set; }
    
    [ProtoMember(6)] public MarketFace? MarketFace { get; set; }
    
    [ProtoMember(8)] public CustomFace? CustomFace { get; set; }
    
    [ProtoMember(12)] public RichMsg? RichMsg { get; set; }
    
    [ProtoMember(19)] public VideoFile? VideoFile { get; set; }
    
    [ProtoMember(45)] public SourceMsg? SrcMsg { get; set; }
    
    [ProtoMember(51)] public LightAppElem? LightAppElem { get; set; }
    
    [ProtoMember(53)] public CommonElem? CommonElem { get; set; }
}

[ProtoPackable]
internal partial class Text
{
    [ProtoMember(1)] public string TextMsg { get; set; }
    
    [ProtoMember(2)] public string Link { get; set; }
    
    [ProtoMember(3)] public byte[] Attr6Buf { get; set; }
    
    [ProtoMember(4)] public byte[] Attr7Buf { get; set; }
    
    [ProtoMember(11)] public byte[] Buf { get; set; }

    [ProtoMember(12)] public ReadOnlyMemory<byte> PbReserve { get; set; }
}

[ProtoPackable]
internal partial class NotOnlineImage
{
    [ProtoMember(1)] public byte[] FilePath { get; set; }

    [ProtoMember(2)] public uint FileLen { get; set; }

    [ProtoMember(3)] public byte[] DownloadPath { get; set; }

    [ProtoMember(4)] public byte[] OldVerSendFile { get; set; }

    [ProtoMember(5)] public uint ImgType { get; set; }

    [ProtoMember(6)] public byte[] PreviewsImage { get; set; }

    [ProtoMember(7)] public byte[] PicMd5 { get; set; }

    [ProtoMember(8)] public uint PicHeight { get; set; }

    [ProtoMember(9)] public uint PicWidth { get; set; }

    [ProtoMember(10)] public byte[] ResId { get; set; }

    [ProtoMember(11)] public byte[] Flag { get; set; }

    [ProtoMember(12)] public string ThumbUrl { get; set; }

    [ProtoMember(13)] public uint Original { get; set; }

    [ProtoMember(14)] public string BigUrl { get; set; }

    [ProtoMember(15)] public string OrigUrl { get; set; }

    [ProtoMember(16)] public uint BizType { get; set; }

    [ProtoMember(17)] public uint Result { get; set; }

    [ProtoMember(18)] public uint Index { get; set; }

    [ProtoMember(19)] public byte[] OpFaceBuf { get; set; }

    [ProtoMember(20)] public bool OldPicMd5 { get; set; }

    [ProtoMember(21)] public uint ThumbWidth { get; set; }

    [ProtoMember(22)] public uint ThumbHeight { get; set; }

    [ProtoMember(23)] public uint FileId { get; set; }

    [ProtoMember(24)] public uint ShowLen { get; set; }

    [ProtoMember(25)] public uint DownloadLen { get; set; }

    [ProtoMember(26)] public string Url400 { get; set; }

    [ProtoMember(27)] public uint Width400 { get; set; }

    [ProtoMember(28)] public uint Height400 { get; set; }

    [ProtoMember(29)] public byte[] PbReserve { get; set; }
}

[ProtoPackable]
internal partial class TransElem
{
    [ProtoMember(1)] public uint ElemType { get; set; }

    [ProtoMember(2)] public byte[] ElemValue { get; set; }
}

[ProtoPackable]
internal partial class MarketFace
{
    [ProtoMember(1)] public string Summary { get; set; }

    [ProtoMember(2)] public uint ItemType { get; set; }

    [ProtoMember(3)] public uint Info { get; set; }

    [ProtoMember(4)] public byte[] FaceId { get; set; }

    [ProtoMember(5)] public uint TabId { get; set; }

    [ProtoMember(6)] public uint SubType { get; set; }

    [ProtoMember(7)] public string Key { get; set; }

    [ProtoMember(10)] public uint Width { get; set; }

    [ProtoMember(11)] public uint Height { get; set; }
}

[ProtoPackable]
internal partial class CustomFace
{
    [ProtoMember(1)] public byte[] Guid { get; set; }
    
    [ProtoMember(2)] public string FilePath { get; set; }
    
    [ProtoMember(3)] public string Shortcut { get; set; }
    
    [ProtoMember(4)] public byte[] Buffer { get; set; }
    
    [ProtoMember(5)] public byte[] Flag { get; set; }
    
    [ProtoMember(6)] public byte[]? OldData { get; set; }
    
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
    
    [ProtoMember(34)] public PbReserve1? PbReserve { get; set; }

    [ProtoPackable]
    public partial class PbReserve1
    {
        [ProtoMember(1)] public int SubType { get; set; }

        [ProtoMember(3)] public int Field3 { get; set; }

        [ProtoMember(4)] public int Field4 { get; set; }

        [ProtoMember(9)] public string Summary { get; set; }

        [ProtoMember(10)] public int Field10 { get; set; }

        [ProtoMember(21)] public PbReserve2 Field21 { get; set; }

        [ProtoMember(31)] public string Field31 { get; set; }
    }

    [ProtoPackable]
    public partial class PbReserve2
    {
        [ProtoMember(1)] public int Field1 { get; set; }

        [ProtoMember(2)] public string Field2 { get; set; }

        [ProtoMember(3)] public int Field3 { get; set; }

        [ProtoMember(4)] public int Field4 { get; set; }

        [ProtoMember(5)] public int Field5 { get; set; }

        [ProtoMember(7)] public string Md5Str { get; set; }
    }
}

[ProtoPackable]
internal partial class RichMsg
{
    [ProtoMember(1)] public ReadOnlyMemory<byte> BytesTemplate1 { get; set; } // Xml模板一
    
    [ProtoMember(2)] public uint ServiceId { get; set; } // 对应的业务标识
    
    [ProtoMember(3)] public ReadOnlyMemory<byte> BytesMsgResid { get; set; } // 长消息resid
    
    [ProtoMember(4)] public uint Rand { get; set; } // 需要删除的消息 rand 值
    
    [ProtoMember(5)] public uint Seq { get; set; } // 需要删除的消息 seq 
    
    [ProtoMember(6)] public uint Flags { get; set; } // bit 0:消息是否来自UrlCache; bit 1:是否包含video标签; bit 2:来自公众帐号的video结构化消息; bit 3:消息在urlencode模块从url转成richmsg
}

[ProtoPackable]
internal partial class VideoFile
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

[ProtoPackable]
internal partial class SourceMsg
{
    [ProtoMember(1)] public ulong[] OrigSeqs { get; set; } // 原消息seq

    [ProtoMember(2)] public ulong SenderUin { get; set; } // 所属发送者uin

    [ProtoMember(3)] public uint Time { get; set; }

    [ProtoMember(4)] public uint Flag { get; set; } // 摘要里是否包括原消息的完整内容0 非完整内容，需要客户端拉取  1 完整内容，不用拉取

    [ProtoMember(5)] public List<ReadOnlyMemory<byte>>? Elems { get; set; } // 消息的种类，包括文本消息，系统表情消息, use ReadOnlyMemory instead of Elem to avoid circular reference

    [ProtoMember(6)] public uint Type { get; set; } // 引用消息展示样式，0表示普通文本，对应解析elems字段；1表示部落类型类似结构化消息的样式，对应解析bytes_richMsg字段

    [ProtoMember(7)] public ReadOnlyMemory<byte> RichMsg { get; set; } // 对应json字段，里面会带上title，summary，coverUrl，jumpUrl等字段

    [ProtoMember(8)] public ReadOnlyMemory<byte> PbReserve { get; set; } // SourceMsg的扩展结构,参见hummer_resv_source_msg.proto      

    [ProtoMember(9)] public ReadOnlyMemory<byte> SrcMsg { get; set; } // 原消息消息体msg_com.Msg	

    [ProtoMember(10)] public ulong ToUin { get; set; } // 所属接收者uin

    [ProtoMember(11)] public ReadOnlyMemory<byte> TroopName { get; set; } // 单独回复原消息群名
}

[ProtoPackable]
internal partial class LightAppElem
{
    [ProtoMember(1)] public ReadOnlyMemory<byte> BytesData { get; set; } // LightApp数据

    [ProtoMember(2)] public byte[] BytesMsgResid { get; set; } // 长消息resid
}

[ProtoPackable]
internal partial class CommonElem
{
    [ProtoMember(1)] public uint ServiceType { get; set; }

    [ProtoMember(2)] public ReadOnlyMemory<byte> PbElem { get; set; }

    [ProtoMember(3)] public uint BusinessType { get; set; }
}