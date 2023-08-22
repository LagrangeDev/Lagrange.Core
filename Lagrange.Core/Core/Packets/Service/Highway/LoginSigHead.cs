using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Service.Highway;

[ProtoContract]
internal class LoginSigHead
{
    [ProtoMember(1)] public uint Uint32LoginSigType { get; set; }
    
    [ProtoMember(2)] public byte[] BytesLoginSig { get; set; } = Array.Empty<byte>();
}