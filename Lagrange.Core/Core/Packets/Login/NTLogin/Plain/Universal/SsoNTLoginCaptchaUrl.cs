using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class SsoNTLoginCaptchaUrl
{
    [ProtoMember(3)] public string Url { get; set; }
    
    [ProtoIgnore] public string Sid => Url.Split("&sid=")[1].Split("&")[0];
}