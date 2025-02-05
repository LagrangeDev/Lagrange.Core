using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class MessageStyleExtra
{
    /// <summary>
    /// If the user is an admin or group owner, this field value is 10315
    /// </summary>
    [ProtoMember(4)] public int Field4 { get; set; } = 10315;
    [ProtoMember(15)] public ulong Font { get; set; }
    [ProtoMember(34)] public uint FontEffectId { get; set; }

    [ProtoMember(51)] public ulong VipType { get; set; } = 337;

    [ProtoMember(52)] public ulong VipLevel { get; set; } = 2;

    [ProtoMember(56)] public ulong VipNameplate { get; set; }

    [ProtoMember(65)] public GroupMemberLevelInfo? GroupMemberLevel { get; set; }

    [ProtoMember(81)] public ulong GroupHonorStyle { get; set; }

    [ProtoMember(107)] public uint MessageSequence { get; set; }

    [ProtoContract]
    internal class GroupMemberLevelInfo
    {
        [ProtoMember(2)] public int Level { get; set; }
    }
}