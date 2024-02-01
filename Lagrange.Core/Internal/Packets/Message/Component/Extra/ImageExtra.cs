using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

/// <summary>
/// Incomplete entity
/// </summary>
[ProtoContract]
internal class ImageExtra
{
    [ProtoMember(1)] public ImageExtraMetadata Metadata { get; set; }
    
    [ProtoMember(2)] public ImageExtraCredential Credential { get; set; }
}

[ProtoContract]
internal class ImageExtraMetadata
{
    [ProtoMember(1)] public ImageExtraFile File { get; set; }
    
    [ProtoMember(2)] public ImageExtraUrls? Urls { get; set; }
}

[ProtoContract]
internal class ImageExtraCredential
{
    [ProtoMember(1)] public ImageExtraCredentialResp Resp { get; set; }
}

[ProtoContract]
internal class ImageExtraFile
{
    [ProtoMember(1)] public ImageExtraFileInfo FileInfo { get; set; }
    
    [ProtoMember(2)] public string? FileUuid { get; set; }
    
    [ProtoMember(3)] public uint Field3 { get; set; }
    
    [ProtoMember(4)] public uint Timestamp { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; }
}

[ProtoContract]
internal class ImageExtraUrls
{
    [ProtoMember(1)] public string Suffix { get; set; }
    
    [ProtoMember(3)] public string Domain { get; set; }
}

[ProtoContract]
internal class ImageExtraCredentialResp
{
    [ProtoMember(1)] public int RetCode { get; set; }
    
    [ProtoMember(11)] public ImageExtraKey? FriendKey { get; set; }
    
    [ProtoMember(12)] public ImageExtraKey? GroupKey { get; set; }
}

[ProtoContract]
internal class ImageExtraKey
{
    [ProtoMember(30)] public string RKey { get; set; }
}

[ProtoContract]
internal class ImageExtraFileInfo
{
    [ProtoMember(1)] public ulong FileSize { get; set; }
    
    [ProtoMember(2)] public string FileMd5 { get; set; }
    
    [ProtoMember(3)] public string FileSha1 { get; set; }
    
    [ProtoMember(4)] public string FilePath { get; set; }
    
    [ProtoMember(6)] public uint PicWidth { get; set; }

    [ProtoMember(7)] public uint PicHeight { get; set; }
}
