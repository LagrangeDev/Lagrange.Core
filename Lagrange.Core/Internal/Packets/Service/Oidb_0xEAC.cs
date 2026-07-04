using Lagrange.Proto;

namespace Lagrange.Core.Internal.Packets.Service;

#pragma warning disable CS8618

[ProtoPackable]
internal partial class EACReqBody
{
    [ProtoMember(1)] public ulong GroupUin { get; set; }
        
    [ProtoMember(2)] public ulong Sequence { get; set; }
        
    [ProtoMember(3)] public uint Random { get; set; }
}

[ProtoPackable]
internal partial class EACRspBody
{
    [ProtoMember(1)] public string Wording { get; set; }
    
    [ProtoMember(2)] public ulong DigestUin { get; set; }
    
    [ProtoMember(3)] public uint DigestTime { get; set; }
    
    // [ProtoMember(4)] public DigestMsg Msg { get; set; }
    
    [ProtoMember(10)] public uint ErrorCode { get; set; }
}
