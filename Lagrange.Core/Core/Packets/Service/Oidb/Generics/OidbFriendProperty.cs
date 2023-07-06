using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Generics;

#pragma warning disable CS8618

[ProtoContract]
internal class OidbFriendProperty
{
    [ProtoMember(1)] public uint Code { get; set; }
    
    [ProtoMember(2)] public string Value { get; set; }
}