using Lagrange.Core.Internal.Packets.System;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message;

#pragma warning disable CS8618

[ProtoContract]
internal class PushMsg
{
    [ProtoMember(1)] public PushMsgBody Message { get; set; }

    [ProtoMember(3)] public int? Status { get; set; }

    [ProtoMember(4)] public NTSysEvent? NtEvent { get; set; }

    [ProtoMember(5)] public int? PingFLag { get; set; }

    [ProtoMember(9)] public int? GeneralFlag { get; set; }
}

[ProtoContract]
internal class PushMsgBody
{
    [ProtoMember(1)] public ResponseHead ResponseHead { get; set; }

    [ProtoMember(2)] public ContentHead ContentHead { get; set; }

    [ProtoMember(3)] public MessageBody? Body { get; set; }
}