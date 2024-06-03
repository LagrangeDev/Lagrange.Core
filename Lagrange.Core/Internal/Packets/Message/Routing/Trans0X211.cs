using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Routing;

[ProtoContract]
internal class Trans0X211
{
    [ProtoMember(1)] public ulong? ToUin { get; set; }
    
    [ProtoMember(2)] public uint? CcCmd { get; set; }
    
    [ProtoMember(8)] public string? Uid { get; set; }
}