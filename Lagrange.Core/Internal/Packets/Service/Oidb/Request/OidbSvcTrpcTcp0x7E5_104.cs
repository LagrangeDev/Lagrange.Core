using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Oidb.Request;

// Resharper Disable InconsistentNaming

/// <summary>
/// Friend Likes
/// </summary>
[ProtoContract]
[OidbSvcTrpcTcp(0x7e5, 104)]
internal class OidbSvcTrpcTcp0x7E5_104
{
    [ProtoMember(11)] public string? TargetUid { get; set; }
    
    [ProtoMember(12)] public uint Field2 { get; set; } // 71
    
    [ProtoMember(13)] public uint Field3 { get; set; } // 1
}