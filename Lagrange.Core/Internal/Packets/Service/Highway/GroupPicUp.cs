using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class GroupPicUp<T>
{
    [ProtoMember(1)] public uint NetType { get; set; }
    
    [ProtoMember(2)] public uint SubCmd { get; set; }
    
    [ProtoMember(3)] public T? Body { get; set; }
}

[ProtoContract]
internal class GroupPicUpRequest
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
    
    [ProtoMember(2)] public uint SrcUin { get; set; }
    
    [ProtoMember(3)] public ulong FileId { get; set; }
    
    [ProtoMember(4)] public byte[]? FileMd5 { get; set; }
    
    [ProtoMember(5)] public ulong FileSize { get; set; }
    
    [ProtoMember(6)] public string? FileName { get; set; }
    
    [ProtoMember(7)] public uint SrcTerm { get; set; } // 2
    
    [ProtoMember(8)] public uint PlatformType { get; set; } // 8
    
    [ProtoMember(9)] public uint BuType { get; set; } // 212
    
    [ProtoMember(10)] public uint PicWidth { get; set; }
    
    [ProtoMember(11)] public uint PicHeight { get; set; }
    
    [ProtoMember(12)] public uint PicType { get; set; } // 1001
    
    [ProtoMember(13)] public string? BuildVer { get; set; } // 1.0.0
    
    [ProtoMember(14)] public uint InnerIp { get; set; }
    
    [ProtoMember(15)] public uint AppPicType { get; set; }
    
    [ProtoMember(16)] public uint OriginalPic { get; set; } // 1
    
    [ProtoMember(17)] public byte[]? FileIndex { get; set; }
    
    [ProtoMember(18)] public ulong DstUin { get; set; }
    
    [ProtoMember(19)] public uint? SrvUpload { get; set; } // 0
    
    [ProtoMember(20)] public byte[]? TransferUrl { get; set; }
    
    [ProtoMember(21)] public ulong MeetGuildId { get; set; }
    
    [ProtoMember(22)] public ulong MeetChannelId { get; set; }
}