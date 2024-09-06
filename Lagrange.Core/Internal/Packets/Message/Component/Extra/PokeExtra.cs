using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class PokeExtra
{
    [ProtoMember(1)] public uint Type { get; set; }
    
    [ProtoMember(7)] public uint Strength { get; set; }
}