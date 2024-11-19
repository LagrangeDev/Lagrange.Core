using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class OldFace
{
    [ProtoMember(1)] public uint? Index { get; set; }
    
    [ProtoMember(2)] public uint? Type { get; set; }
    
}