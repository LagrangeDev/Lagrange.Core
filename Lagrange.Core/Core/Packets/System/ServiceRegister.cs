using ProtoBuf;

namespace Lagrange.Core.Core.Packets.System;

/// <summary>
/// trpc.qq_new_tech.status_svc.StatusService.Register
/// </summary>
[ProtoContract]
internal class ServiceRegister
{
    [ProtoMember(1)] public string? Guid { get; set; }
    
    [ProtoMember(2)] public int? Type { get; set; }
    
    [ProtoMember(3)] public string? CurrentVersion { get; set; }
    
    [ProtoMember(4)] public int? Field4 { get; set; }
    
    [ProtoMember(5)] public int? Field5 { get; set; } // 2052
    
    [ProtoMember(6)] public OnlineOsInfo? Online { get; set; }
    
    [ProtoMember(7)] public int? Field7 { get; set; }
    
    [ProtoMember(8)] public int? Field8 { get; set; }
    
    [ProtoMember(9)] public int? Field9 { get; set; }
}