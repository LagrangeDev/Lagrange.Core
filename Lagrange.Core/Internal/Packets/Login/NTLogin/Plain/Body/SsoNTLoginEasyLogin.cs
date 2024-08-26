using Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;
using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Body;

// ReSharper disable once InconsistentNaming

[ProtoContract]
internal class SsoNTLoginEasyLogin
{
    [ProtoMember(1)] public byte[]? TempPassword { get; set; }

    [ProtoMember(2)] public SsoNTLoginCaptchaSubmit? Captcha { get; set; }
}