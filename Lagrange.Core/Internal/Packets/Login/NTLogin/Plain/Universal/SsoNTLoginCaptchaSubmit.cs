using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class SsoNTLoginCaptchaSubmit
{
    [ProtoMember(1)] public string Ticket { get; set; }

    [ProtoMember(2)] public string RandStr { get; set; }

    [ProtoMember(3)] public string Aid { get; set; }
}