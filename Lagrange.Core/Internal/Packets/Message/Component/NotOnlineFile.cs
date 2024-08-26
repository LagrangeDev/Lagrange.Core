using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component;

[ProtoContract]
internal class NotOnlineFile
{
    [ProtoMember(1)] public int? FileType { get; set; }

    [ProtoMember(2)] public byte[]? Sig { get; set; }

    [ProtoMember(3)] public string? FileUuid { get; set; }

    [ProtoMember(4)] public byte[]? FileMd5 { get; set; }

    [ProtoMember(5)] public string? FileName { get; set; }

    [ProtoMember(6)] public long? FileSize { get; set; }

    [ProtoMember(7)] public byte[]? Note { get; set; }

    [ProtoMember(8)] public int? Reserved { get; set; }

    [ProtoMember(9)] public int? Subcmd { get; set; }

    [ProtoMember(10)] public int? MicroCloud { get; set; }

    [ProtoMember(11)] public List<byte[]>? BytesFileUrls { get; set; }

    [ProtoMember(12)] public int? DownloadFlag { get; set; }

    [ProtoMember(50)] public int? DangerEvel { get; set; }

    [ProtoMember(51)] public int? LifeTime { get; set; }

    [ProtoMember(52)] public int? UploadTime { get; set; }

    [ProtoMember(53)] public int? AbsFileType { get; set; }

    [ProtoMember(54)] public int? ClientType { get; set; }

    [ProtoMember(55)] public int? ExpireTime { get; set; }

    [ProtoMember(56)] public byte[]? PbReserve { get; set; }

    [ProtoMember(57)] public string? FileHash { get; set; }
}