using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

/// <summary>
/// Set Friend Request
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0xB5D, 44)]
public class OidbSvcTrpcTcp0xB5D_44
{
    [ProtoMember(1)] public uint Accept { get; set; }  // 3 for accept, 5 for reject
    
    [ProtoMember(2)] public string TargetUid { get; set; }
}