using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Get Cookie
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x102A, 0)]
internal class OidbSvcTrpcTcp0x102A_0
{
    [ProtoMember(1)] public List<string> Domain { get; set; }
}