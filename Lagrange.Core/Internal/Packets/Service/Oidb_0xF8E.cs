using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class DF8EReqBody
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }
}

[ProtoPackable]
internal partial class DF8ERspBody
{
    [ProtoMember(1)] public F8EInfoValue? Info { get; set; }

    [ProtoMember(2)] public List<F8EUserNode>? GroupList { get; set; }

    [ProtoMember(3)] public uint ExpirationTime { get; set; }
}

[ProtoPackable]
internal partial class F8EInfoValue
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong Sequence { get; set; }

    [ProtoMember(3)] public uint Random { get; set; }

    [ProtoMember(4)] public ulong Uin { get; set; }

    [ProtoMember(5)] public string Nickname { get; set; }

    [ProtoMember(6)] public string? Title { get; set; }

    [ProtoMember(7)] public string JumpUrl { get; set; }

    [ProtoMember(8)] public string IconUrl { get; set; }

    [ProtoMember(9)] public uint CreateTime { get; set; }

    [ProtoMember(10)] public string AppName { get; set; }

    [ProtoMember(11)] public ulong AppId { get; set; }

    [ProtoMember(12)] public int MessageType { get; set; }
}

[ProtoPackable]
internal partial class F8EUserNode
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong Sequence { get; set; }

    [ProtoMember(3)] public uint Status { get; set; }
}
