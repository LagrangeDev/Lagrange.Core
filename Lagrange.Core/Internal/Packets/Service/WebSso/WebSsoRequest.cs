using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.WebSso;

// Resharper disable InconsistentNaming
#pragma warning disable CS8618

// ref https://github.com/Mrs4s/MiraiGo/blob/54bdd873e3fed9fe1c944918924674dacec5ac76/client/pb/web/WebSsoBody.proto
[ProtoContract]
internal class WebSsoRequest
{
    [ProtoMember(1)] public string? Version { get; set; }

    [ProtoMember(2)] public uint? Type { get; set; }

    [ProtoMember(3)] public string? Data { get; set; }

    [ProtoMember(4)] public string? WebData { get; set; }
}