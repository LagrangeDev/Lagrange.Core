using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbGroup
{
    [ProtoMember(1)] public uint GroupId { get; set; }
    
    [ProtoMember(2)] public string GroupName { get; set; }
}