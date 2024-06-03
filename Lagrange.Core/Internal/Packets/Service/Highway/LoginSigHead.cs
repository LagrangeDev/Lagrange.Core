using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Service.Highway;

[ProtoContract]
internal class LoginSigHead
{
    [ProtoMember(1)] public uint Uint32LoginSigType { get; set; }
    
    [ProtoMember(2)] public byte[] BytesLoginSig { get; set; } = Array.Empty<byte>();
    
    [ProtoMember(3)] public uint AppId { get; set; }
}