using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbFriend
{
    [ProtoMember(1)] public string Uid { get; set; }
    
    [ProtoMember(3)] public uint Uin { get; set; }
    
    [ProtoMember(10001)] public OidbFriendAdditional Additional { get; set; }
}