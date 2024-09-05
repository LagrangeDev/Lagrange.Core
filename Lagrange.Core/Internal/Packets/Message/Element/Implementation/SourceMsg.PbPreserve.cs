using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

internal partial class SrcMsg
{
    [ProtoContract]
    internal class Preserve
    {
        [ProtoMember(3)] public ulong MessageId { get; set; }

        [ProtoMember(6)] public string? SenderUid { get; set; }

        [ProtoMember(7)] public string? ReceiverUid { get; set; }

        [ProtoMember(8)] public uint? FriendSequence { get; set; }
    }
}