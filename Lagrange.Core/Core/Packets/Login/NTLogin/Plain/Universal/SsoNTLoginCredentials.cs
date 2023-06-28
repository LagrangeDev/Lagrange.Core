using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Login.NTLogin.Plain.Universal;

// ReSharper disable once InconsistentNaming
#pragma warning disable CS8618

[ProtoContract]
internal class SsoNTLoginCredentials
{
    [ProtoMember(3)] public byte[] TempPassword { get; set; }
    
    [ProtoMember(4)] public byte[] Tgt { get; set; }

    [ProtoMember(5)] public byte[] D2 { get; set; }
    
    [ProtoMember(6)] public byte[] D2Key { get; set; }
}