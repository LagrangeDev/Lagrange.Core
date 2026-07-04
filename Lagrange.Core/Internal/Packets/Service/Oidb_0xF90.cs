using Lagrange.Proto;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Service;

[ProtoPackable]
internal partial class F90ReqBody
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong Seq { get; set; }

    [ProtoMember(3)] public ulong Random { get; set; }

    [ProtoMember(4)] public ulong AppId { get; set; }
}

[ProtoPackable]
internal partial class F90RspBody
{
    [ProtoMember(1)] public F90Info Info { get; set; }
}

[ProtoPackable]
internal partial class F90Info
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong Seq { get; set; }

    [ProtoMember(3)] public uint Random { get; set; }

    [ProtoMember(4)] public ulong Uin { get; set; }

    [ProtoMember(5)] public string Nickname { get; set; }

    [ProtoMember(6)] public string Title { get; set; }

    [ProtoMember(7)] public string JumpUrl { get; set; }

    [ProtoMember(8)] public string IconUrl { get; set; }

    [ProtoMember(9)] public uint CreateTime { get; set; }

    [ProtoMember(10)] public string AppName { get; set; }

    [ProtoMember(11)] public ulong AppId { get; set; }

    [ProtoMember(12)] public uint MsgType { get; set; }
}
