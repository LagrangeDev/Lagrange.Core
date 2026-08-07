using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D1253ReqBody
{
    [ProtoPackable]
    internal partial class MuteInfo
    {
        [ProtoMember(1)] public string TargetUid { get; set; }

        [ProtoMember(2)] public uint Duration { get; set; }
    }

    [ProtoMember(1)] public long GroupUin { get; set; }

    [ProtoMember(2)] public uint Type { get; set; }

    [ProtoMember(3)] public MuteInfo Body { get; set; }
}

[ProtoPackable]
internal partial class D1253RspBody
{
    [ProtoMember(2)] public string? Success { get; set; }
}
