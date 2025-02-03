using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class MessageStyleExtra
{
    [ProtoMember(15)] public ulong Font { get; set; }
    [ProtoMember(34)] public uint FontEffectId { get; set; }

    [ProtoMember(51)] public ulong VipType { get; set; } = 337;

    [ProtoMember(52)] public ulong VipLevel { get; set; } = 2;

    [ProtoMember(56)] public ulong VipNameplate { get; set; }

    [ProtoMember(65)] public GroupMemberLevelInfo? GroupMemberLevel { get; set; }

    [ProtoMember(66)] public int Field66 { get; set; } = 128;

    [ProtoMember(71)] public int Field71 { get; set; } = 2; // 258;

    [ProtoMember(79)] public uint Field79 { get; set; } = 131080;

    [ProtoMember(81)] public ulong GroupHonorStyle { get; set; }

    [ProtoMember(107)] public uint MessageSequence { get; set; }

    [ProtoContract]
    internal class GroupMemberLevelInfo
    {
        [ProtoMember(2)] public int Level { get; set; }
    }
}