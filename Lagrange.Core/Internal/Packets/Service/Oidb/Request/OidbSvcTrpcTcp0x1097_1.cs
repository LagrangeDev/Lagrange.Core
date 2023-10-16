using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

/// <summary>
/// Quit Group
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x1097, 1)]
internal class OidbSvcTrpcTcp0x1097_1
{
    [ProtoMember(1)] public uint GroupUin { get; set; }
}