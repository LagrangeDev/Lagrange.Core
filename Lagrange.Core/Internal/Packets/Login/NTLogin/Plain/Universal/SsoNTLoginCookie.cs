using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class SsoNTLoginCookie
{
    [ProtoMember(1)] public string? Cookie { get; set; }
}