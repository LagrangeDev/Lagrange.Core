using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable once InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class SsoNTLoginUid
{
    [ProtoMember(2)] public string Uid { get; set; }
}