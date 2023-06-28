using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Body;

// ReSharper disable once InconsistentNaming

[ProtoContract]
internal class SsoNTLoginEasyLogin
{
    [ProtoMember(1)] public byte[]? TempPassword { get; set; }
}