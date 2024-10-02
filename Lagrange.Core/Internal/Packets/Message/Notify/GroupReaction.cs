using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class GroupReactionData0
{
    // What the fuck was tx thinking?
    [ProtoMember(1)] public GroupReactionData1 Data { get; set; }
}

[ProtoContract]
internal class GroupReactionData1
{
    [ProtoMember(1)] public GroupReactionData2 Data { get; set; }
}

[ProtoContract]
internal class GroupReactionData2
{
    [ProtoMember(2)] public GroupReactionTarget Target { get; set; }

    [ProtoMember(3)] public GroupReactionData3 Data { get; set; }
}

[ProtoContract]
internal class GroupReactionTarget
{
    [ProtoMember(1)] public uint Sequence { get; set; }
}

[ProtoContract]
internal class GroupReactionData3
{
    [ProtoMember(1)] public string Code { get; set; }

    [ProtoMember(3)] public uint Count { get; set; }

    [ProtoMember(4)] public string OperatorUid { get; set; }

    [ProtoMember(5)] public uint Type { get; set; } // 1 Add 2 Remove
}

