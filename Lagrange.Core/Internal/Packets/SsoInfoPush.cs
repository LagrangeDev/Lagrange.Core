using ProtoBuf;

namespace Lagrange.Core.Internal.Packets;

[ProtoContract]
internal class SsoInfoPush<T> where T : class
{
    [ProtoMember(3)] public uint Type { get; set; }
    
    [ProtoMember(4)] public uint RandomSeq { get; set; }
    
    [ProtoMember(6)] public T? Info { get; set; }
}

[ProtoContract]
internal class SsoInfoPushData
{
    [ProtoMember(3)] public uint Type { get; set; }
    
    [ProtoMember(4)] public uint RandomSeq { get; set; }
}