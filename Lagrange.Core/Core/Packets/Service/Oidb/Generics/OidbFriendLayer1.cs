using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Generics;

#pragma warning disable CS8618

[ProtoContract]
internal class OidbFriendLayer1
{
    [ProtoMember(2)] public List<OidbFriendProperty> Properties { get; set; }
}