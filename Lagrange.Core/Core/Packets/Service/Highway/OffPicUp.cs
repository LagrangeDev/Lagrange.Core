using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Highway;

/// <summary>
/// LongConn.OffPicUp
/// </summary>
[ProtoContract]
internal class OffPicUp<T>
{
    [ProtoMember(1)] public uint SubCmd { get; set; } // 1
    
    [ProtoMember(2)] public T? Info { get; set; }
    
    [ProtoMember(3)] public uint NetType { get; set; } // 3
}

[ProtoContract]
internal class OffPicUpRequest
{
    [ProtoMember(1)] public uint SrcUin { get; set; }
    
    // [ProtoMember(2)] public uint DstUin { get; set; } // Ignore
    
    [ProtoMember(3)] public ulong FileId { get; set; } // 1
    
    [ProtoMember(4)] public byte[]? FileMd5 { get; set; }
    
    [ProtoMember(5)] public ulong FileSize { get; set; }
    
    [ProtoMember(6)] public string? FileName { get; set; }
    
    [ProtoMember(7)] public uint SrcTerm { get; set; } // 2
    
    [ProtoMember(8)] public uint PlatformType { get; set; } // 8
    
    // [ProtoMember(9)] public uint InnerIp { get; set; } // Ignore
    
    [ProtoMember(10)] public bool AddressBook { get; set; } // false
    
    // [ProtoMember(11)] public uint Retry { get; set; } // Ignore
    
    [ProtoMember(12)] public uint BuType { get; set; } // 8
    
    [ProtoMember(13)] public bool PicOriginal { get; set; } // 1
    
    [ProtoMember(14)] public uint PicWidth { get; set; }
    
    [ProtoMember(15)] public uint PicHeight { get; set; }
    
    [ProtoMember(16)] public uint PicType { get; set; } // 1001
    
    // [ProtoMember(17)] public byte[]? BuildVer { get; set; } // Ignore
    
    // [ProtoMember(18)] public byte[]? FileIndex { get; set; } // Ignore
    
    // [ProtoMember(19)] public uint StoreDays { get; set; } // Ignore
    
    // [ProtoMember(20)] public uint TryupStepflag { get; set; } // Ignore
    
    // [ProtoMember(21)] public bool RejectTryfast { get; set; } // Ignore
    
    [ProtoMember(22)] public uint SrvUpload { get; set; } // 0
    
    // [ProtoMember(23)] public byte[]? TransferUrl { get; set; } // Ignore
    
    [ProtoMember(25)] public string? TargetUid { get; set; }
}