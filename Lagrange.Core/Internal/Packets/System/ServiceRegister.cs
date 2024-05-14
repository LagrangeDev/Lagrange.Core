using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

/// <summary>
/// trpc.qq_new_tech.status_svc.StatusService.Register
/// </summary>
[ProtoContract]
internal class ServiceRegister
{
    [ProtoMember(1)] public string? Guid { get; set; }
    
    [ProtoMember(2)] public int? KickPC { get; set; }
    
    [ProtoMember(3)] public string? CurrentVersion { get; set; }
    
    [ProtoMember(4)] public int? IsFirstRegisterProxyOnline { get; set; }
    
    [ProtoMember(5)] public int? LocaleId { get; set; } // 2052
    
    [ProtoMember(6)] public OnlineDeviceInfo? Device { get; set; }
    
    [ProtoMember(7)] public int? SetMute { get; set; }
    
    [ProtoMember(8)] public int? RegisterVendorType { get; set; }
    
    [ProtoMember(9)] public int? RegType { get; set; }
}