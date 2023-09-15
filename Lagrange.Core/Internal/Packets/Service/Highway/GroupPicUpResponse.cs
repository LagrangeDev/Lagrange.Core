using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class GroupPicUpResponse
{
    [ProtoMember(1)] public ulong FileId { get; set; }
    
    [ProtoMember(2)] public uint Result { get; set; }
    
    [ProtoMember(3)] public byte[]? FailMsg { get; set; }
    
    [ProtoMember(4)] public bool FileExit { get; set; }
    
    // [ProtoMember(5)] public ImgInfo? ImgInfo { get; set; }
    
    [ProtoMember(6)] public List<uint>? UpIp { get; set; }
    
    [ProtoMember(7)] public List<uint>? UpPort { get; set; }
    
    [ProtoMember(8)] public byte[]? UpUkey { get; set; }
    
    [ProtoMember(9)] public ulong Fileid { get; set; }
    
    [ProtoMember(10)] public ulong UpOffset { get; set; }
    
    [ProtoMember(11)] public ulong BlockSize { get; set; }
    
    [ProtoMember(12)] public bool NewBigChan { get; set; }
    
    // [ProtoMember(26)] public IPv6Info[]? UpIp6 { get; set; }
    
    [ProtoMember(27)] public byte[]? ClientIp6 { get; set; }
    
    [ProtoMember(28)] public byte[]? DownloadIndex { get; set; }
}