using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper Disable InconsistentNaming

/// <summary>
/// Group add and reduce Reaction
/// </summary>
[ProtoContract]
internal class OidbSvcTrpcTcp0x9082
{
    [ProtoMember(2)] public uint GroupUin { get; set; }

    [ProtoMember(3)] public uint Sequence { get; set; }

    [ProtoMember(4)] public string? Code { get; set; }

    [ProtoMember(5)] public bool Field5 { get; set; } // true

    [ProtoMember(6)] public bool Field6 { get; set; } // false

    [ProtoMember(7)] public bool Field7 { get; set; } // false
}