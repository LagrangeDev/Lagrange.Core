using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupReactMessageWithEmoji
{
    [ProtoMember(1)] public GroupReactMessageWithEmoji_1 Field1 { get; set; }
}

[ProtoContract]
internal class GroupReactMessageWithEmoji_1
{
    [ProtoMember(1)] public GroupReactMessageWithEmoji_2 Field1 { get; set; }
}

[ProtoContract]
internal class GroupReactMessageWithEmoji_2
{
    [ProtoMember(1)] public uint Field1 { get; set; }
    [ProtoMember(2)] public GroupReactMessageWithEmoji_3 Field2 { get; set; }
    [ProtoMember(3)] public GroupReactMessageWithEmoji_4 Field3 { get; set; }
}

[ProtoContract]
internal class GroupReactMessageWithEmoji_3
{
    [ProtoMember(1)] public uint Sequence { get; set; }
    [ProtoMember(2)] public uint Field2 { get; set; }
    [ProtoMember(3)] public uint Field3 { get; set; }
}

[ProtoContract]
internal class GroupReactMessageWithEmoji_4
{
    [ProtoMember(1)] public string FaceId { get; set; }
    [ProtoMember(2)] public uint Field2 { get; set; }
    [ProtoMember(3)] public bool IsSet { get; set; }
    [ProtoMember(4)] public string OperatorUid { get; set; }
    [ProtoMember(5)] public uint Field5 { get; set; }
}