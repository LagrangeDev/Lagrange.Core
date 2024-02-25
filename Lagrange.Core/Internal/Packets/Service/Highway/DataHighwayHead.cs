using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class DataHighwayHead
{
    [ProtoMember(1)] public uint Version { get; set; }
    
    [ProtoMember(2)] public string? Uin { get; set; }
    
    [ProtoMember(3)] public string? Command { get; set; }
    
    [ProtoMember(4)] public uint Seq { get; set; }
    
    [ProtoMember(5)] public uint RetryTimes { get; set; }
    
    [ProtoMember(6)] public uint AppId { get; set; }
    
    [ProtoMember(7)] public uint DataFlag { get; set; }
    
    [ProtoMember(8)] public uint CommandId { get; set; }
    
    [ProtoMember(9)] public byte[]? BuildVer { get; set; }
    
    // [ProtoMember(10)] public uint LocaleId { get; set; }
    
    // [ProtoMember(11)] public uint EnvId { get; set; }
}