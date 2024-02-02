using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

/// <summary>
/// Video Download
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x11e9, 200)]
internal class OidbSvcTrpcTcp0x11E9_200
{
    [ProtoMember(1)] public OidbSvcTrpcTcp0x126D_200Field1 Field1 { get; set; }
    
    [ProtoMember(3)] public OidbSvcTrpcTcp0x126D_200Field3 Field3 { get; set; }
}