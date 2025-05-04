using System.Diagnostics.CodeAnalysis;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class SsoGetGroupMsgResponse
{
    [ProtoMember(3)] public SsoGetGroupMsgResponseBody Body { get; set; }
}

[ProtoContract]
internal class SsoGetGroupMsgResponseBody
{
    [ProtoMember(1)] public uint Retcode { get; set; }

    [ProtoMember(2)] public string Message { get; set; }

    [ProtoMember(3)] public uint GroupUin { get; set; }

    [ProtoMember(4)] public uint StartSequence { get; set; }

    [ProtoMember(5)] public uint EndSequence { get; set; }

    [ProtoMember(6)] public List<PushMsgBody>? Messages { get; set; }
}