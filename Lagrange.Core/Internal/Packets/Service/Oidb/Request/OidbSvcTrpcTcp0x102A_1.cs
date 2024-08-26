using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming

/// <summary>
/// Fetch Client Key: 我也觉得抽象 可是他真的是空的
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x102A, 1)]
internal class OidbSvcTrpcTcp0x102A_1
{

}