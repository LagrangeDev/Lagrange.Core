using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class SsoNTLoginSystem
{
    [ProtoMember(1)] public string? Os { get; set; }
    
    [ProtoMember(2)] public string? DeviceName { get; set; }

    [ProtoMember(3)] public int Type { get; set; } = 7; // ?
    
    [ProtoMember(4)] public byte[]? Guid { get; set; }
}