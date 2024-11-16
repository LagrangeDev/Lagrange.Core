using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

// Stupid TX
// TODO: Currently only supports PinChanged
[ProtoContract]
internal class FriendDeleteOrPinChanged
{
    [ProtoMember(1)] public FriendDeleteOrPinChangedBody Body { get; set; }
}

[ProtoContract]
internal class FriendDeleteOrPinChangedBody
{
    // Maybe is type, need check
    // 7 Pin changed
    // 5 Friend delete
    [ProtoMember(2)] public uint Type { get; set; }

    [ProtoMember(20)] public PinChanged? PinChanged { get; set; }
}

[ProtoContract]
internal class PinChanged
{
    [ProtoMember(1)] public PinChangedBody Body { get; set; }
}

[ProtoContract]
internal class PinChangedBody
{
    [ProtoMember(1)] public string Uid { get; set; }

    [ProtoMember(2)] public uint? GroupUin { get; set; }

    [ProtoMember(400)] public PinChangedInfo Info { get; set; }
}

[ProtoContract]
internal class PinChangedInfo
{
    // if (Timestamp.Length != 0)   pin
    // if (Timestamp.Length == 0)   unpin
    [ProtoMember(2)] public byte[] Timestamp { get; set; }
}