using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

/// <summary>
/// trpc.qq_new_tech.status_svc.StatusService.SetStatus
/// </summary>
[ProtoContract]
internal class SetStatus
{
    [ProtoMember(1)] public uint Field1 { get; set; } // 10

    [ProtoMember(2)] public uint Status { get; set; }

    [ProtoMember(3)] public uint ExtStatus { get; set; }

    [ProtoMember(4)] public SetStatusCustomExt? CustomExt { get; set; }
}

[ProtoContract]
internal class SetStatusCustomExt
{
    [ProtoMember(1)] public uint FaceId { get; set; }

    [ProtoMember(2)] public string? Text { get; set; }

    [ProtoMember(3)] public uint Field3 { get; set; } // 1
}

[ProtoContract]
internal class SetStatusResponse
{
    [ProtoMember(2)] public string Message { get; set; } = string.Empty;
}