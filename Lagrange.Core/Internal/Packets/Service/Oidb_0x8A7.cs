using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D8A7ReqBody
{
    [ProtoMember(1)] public uint SubCommand { get; set; }

    [ProtoMember(2)] public uint LimitIntervalTypeForUin { get; set; }

    [ProtoMember(3)] public uint LimitIntervalTypeForGroup { get; set; }

    [ProtoMember(4)] public ulong Uin { get; set; }

    [ProtoMember(5)] public ulong GroupCode { get; set; }
}

[ProtoPackable]
internal partial class D8A7RspBody
{
    [ProtoMember(1)] public bool CanAtAll { get; set; }

    [ProtoMember(2)] public uint RemainAtAllCountForUin { get; set; }

    [ProtoMember(3)] public uint RemainAtAllCountForGroup { get; set; }

    [ProtoMember(4)] public byte[] PromptMessage1 { get; set; }

    [ProtoMember(5)] public byte[] PromptMessage2 { get; set; }

    [ProtoMember(6)] public bool ShowAtAllLabel { get; set; }
}
