using Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;
using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain;

// ReSharper disable once InconsistentNaming

/// <summary>
/// Response for trpc.login.ecdh.EcdhService.SsoNTLoginPasswordLogin
/// </summary>
[ProtoContract]
internal class SsoNTLoginResponse
{
    [ProtoMember(1)] public SsoNTLoginCredentials? Credentials { get; set; }
    
    [ProtoMember(2)] public SsoNTLoginCaptchaUrl? Captcha { get; set; }
    
    [ProtoMember(3)] public SsoNTLoginUnusual? Unusual { get; set; }
    
    [ProtoMember(4)] public SsoNTLoginUid? Uid { get; set; }
}