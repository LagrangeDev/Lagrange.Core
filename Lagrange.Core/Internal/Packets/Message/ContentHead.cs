using Lagrange.Core.Internal.Packets.Message.Routing;
using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message;

[ProtoContract]
internal class ContentHead
{
    [ProtoMember(1)] public uint Type { get; set; }

    [ProtoMember(2)] public uint? SubType { get; set; }

    [ProtoMember(3)] public uint? DivSeq { get; set; }

    [ProtoMember(4)] public long? MsgId { get; set; }

    [ProtoMember(5)] public uint? Sequence { get; set; }

    [ProtoMember(6)] public long? Timestamp { get; set; }

    [ProtoMember(7)] public long? Field7 { get; set; }

    [ProtoMember(8)] public uint? Field8 { get; set; }

    [ProtoMember(9)] public uint? Field9 { get; set; }

    [ProtoMember(12)] public ulong? NewId { get; set; }

    [ProtoMember(15)] public ForwardHead? Forward { get; set; }
}