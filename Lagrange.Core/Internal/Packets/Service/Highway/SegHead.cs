using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class SegHead
{
    [ProtoMember(1)] public uint ServiceId { get; set; }
    
    [ProtoMember(2)] public ulong Filesize { get; set; }
    
    [ProtoMember(3)] public ulong DataOffset { get; set; }
    
    [ProtoMember(4)] public uint DataLength { get; set; }
    
    [ProtoMember(5)] public uint RetCode { get; set; }

    [ProtoMember(6)] public byte[] ServiceTicket { get; set; } = Array.Empty<byte>();
    
    // [ProtoMember(7)] public uint Flag { get; set; }
    
    [ProtoMember(8)] public byte[] Md5 { get; set; } = Array.Empty<byte>();
    
    [ProtoMember(9)] public byte[] FileMd5 { get; set; } = Array.Empty<byte>();
    
    [ProtoMember(10)] public uint CacheAddr { get; set; }
    
    // [ProtoMember(11)] public uint QueryTimes { get; set; }
    
    // [ProtoMember(12)] public uint UpdateCacheIp { get; set; }
    
    [ProtoMember(13)] public uint CachePort { get; set; }
}