using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupFileExtra
{
    [ProtoMember(1)] public uint Field1 { get; set; }
    
    [ProtoMember(2)] public string FileName { get; set; }
    
    [ProtoMember(3)] public string Display { get; set; }
    
    [ProtoMember(7)] public GroupFileExtraInner Inner { get; set; }
}

[ProtoContract]
internal class GroupFileExtraInner
{
    [ProtoMember(2)] public GroupFileExtraInfo Info { get; set; }
}

[ProtoContract]
internal class GroupFileExtraInfo
{
    [ProtoMember(1)] public uint BusId { get; set; }
    
    [ProtoMember(2)] public string FileId { get; set; }
    
    [ProtoMember(3)] public long FileSize { get; set; }
    
    [ProtoMember(4)] public string FileName { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; }
    
    [ProtoMember(7)] public string Field7 { get; set; }

    [ProtoMember(8)] public string FileMd5 { get; set; }  // hexed
}