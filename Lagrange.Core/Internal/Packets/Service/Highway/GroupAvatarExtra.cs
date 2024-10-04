using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;
#pragma warning disable CS8618

[ProtoContract]
internal class GroupAvatarExtra
{
    [ProtoMember(1)] public uint Type { get; set; } // 101
    
    [ProtoMember(2)] public uint GroupUin { get; set; }
    
    [ProtoMember(3)] public GroupAvatarExtraField3 Field3 { get; set; }
    
    [ProtoMember(5)] public uint Field5 { get; set; } // 3
    
    [ProtoMember(6)] public uint Field6 { get; set; } // 1
}

[ProtoContract]
internal class GroupAvatarExtraField3
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 1
}