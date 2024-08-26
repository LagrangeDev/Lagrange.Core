using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbTwoNumber
{
    [ProtoMember(1)] public uint Number1 { get; set; }

    [ProtoMember(2)] public uint Number2 { get; set; }
}