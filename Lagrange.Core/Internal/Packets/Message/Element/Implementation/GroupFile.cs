using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class GroupFile
{
    [ProtoMember(1)] public byte[] Filename { get; set; }

    [ProtoMember(2)] public long FileSize { get; set; }

    [ProtoMember(3)] public byte[] FileId { get; set; }

    [ProtoMember(4)] public byte[] BatchId { get; set; }

    [ProtoMember(5)] public byte[] FileKey { get; set; }

    [ProtoMember(6)] public byte[] Mark { get; set; }

    [ProtoMember(7)] public long Sequence { get; set; }

    [ProtoMember(8)] public byte[] BatchItemId { get; set; }

    [ProtoMember(9)] public int FeedMsgTime { get; set; }

    [ProtoMember(10)] public byte[] PbReserve { get; set; }
}