using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupChange
{
    [ProtoMember(1)] public uint GroupUin { get; set; }

    [ProtoMember(2)] public uint Flag { get; set; }

    [ProtoMember(3)] public string MemberUid { get; set; }

    [ProtoMember(4)] public uint DecreaseType { get; set; } // 131 Kick 130 Exit

    [ProtoMember(5)] public byte[]? Operator { get; set; }

    [ProtoMember(6)] public uint IncreaseType { get; set; }

    [ProtoMember(7)] public byte[]? Field7 { get; set; }
}

[ProtoContract]
internal class OperatorInfo
{
    [ProtoMember(1)] public OperatorField1 Operator { get; set; }
}

[ProtoContract]
internal class OperatorField1
{
    [ProtoMember(1)] public string? Uid { get; set; }

    [ProtoMember(2)] public uint Field2 { get; set; }

    [ProtoMember(3)] public byte[]? Field3 { get; set; }

    [ProtoMember(4)] public uint Field4 { get; set; }

    [ProtoMember(5)] public byte[]? Field5 { get; set; }
}