using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;

#pragma warning disable CS8618
// ReSharper disable InconsistentNaming

[ProtoContract]
internal class SsoNTLoginError
{
    [ProtoMember(1)] public uint ErrorCode { get; set; }
    
    [ProtoMember(2)] public string Tag { get; set; }
    
    [ProtoMember(3)] public string Message { get; set; }
    
    [ProtoMember(5)] public string? NewDeviceVerifyUrl { get; set; }
}