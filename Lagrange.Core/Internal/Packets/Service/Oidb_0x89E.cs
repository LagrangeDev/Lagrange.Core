using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class D89EReqBody
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong OldOwner { get; set; }

    [ProtoMember(3)] public ulong NewOwner { get; set; }
}

[ProtoPackable]
internal partial class D89ERspBody
{
    [ProtoMember(1)] public ulong GroupCode { get; set; }

    [ProtoMember(2)] public ulong OldOwner { get; set; }

    // The TIM definition declares this field as PBUInt32Field despite its uint64_ name.
    [ProtoMember(3)] public uint NewOwner { get; set; }

    [ProtoMember(4)] public uint Result { get; set; }

    [ProtoMember(5)] public string ErrorInfo { get; set; }
}
