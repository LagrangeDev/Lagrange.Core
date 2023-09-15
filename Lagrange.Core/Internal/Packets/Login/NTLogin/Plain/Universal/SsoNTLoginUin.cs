using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable InconsistentNaming

[ProtoContract]
internal class SsoNTLoginUin
{
    [ProtoMember(1)] public string? Uin { get; set; }
}