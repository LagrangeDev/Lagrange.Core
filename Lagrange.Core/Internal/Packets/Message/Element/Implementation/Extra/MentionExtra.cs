using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// <see cref="Text"/>, ProtoMember12 (PbReserve)
/// </summary>
[ProtoContract]
internal class MentionExtra
{
    [ProtoMember(3)] public int Type { get; set; } // 2

    [ProtoMember(4)] public uint Uin { get; set; }

    [ProtoMember(5)] public int Field5 { get; set; } // Const Zero

    [ProtoMember(9)] public string Uid { get; set; } // set this string to "all" to mention everyone in the Group
}