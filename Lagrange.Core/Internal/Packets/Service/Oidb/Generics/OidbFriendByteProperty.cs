using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Generics;

#pragma warning disable CS8618

[ProtoContract]
internal class OidbFriendByteProperty
{
    [ProtoMember(1)] public uint Code { get; set; }
    
    [ProtoMember(2)] public byte[] Value { get; set; }
}