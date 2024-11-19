using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.WebSso;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

// ref https://github.com/Mrs4s/MiraiGo/blob/54bdd873e3fed9fe1c944918924674dacec5ac76/client/pb/web/WebSsoBody.proto
[ProtoContract]
internal class STServiceMonitItem
{
    [ProtoMember(1)] public string? Cmd { get; set; }

    [ProtoMember(2)] public string? URL { get; set; }

    [ProtoMember(3)] public int? ErrCode { get; set; }

    [ProtoMember(4)] public uint? Cost { get; set; }

    [ProtoMember(5)] public uint? Src { get; set; }
}

[ProtoContract]
internal class STServiceMonitReq
{
    [ProtoMember(1)] public STServiceMonitItem[] list { get; set; }
}