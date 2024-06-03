using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class ServiceKickNTResponse
{
    [ProtoMember(1)] public uint Uin { get; set; }
    
    [ProtoMember(3)] public string Tips { get; set; }
    
    [ProtoMember(4)] public string Title { get; set; }
    
    [ProtoMember(5)] public int Field5 { get; set; }
    
    [ProtoMember(6)] public int Field6 { get; set; }
    
    [ProtoMember(8)] public int Field8 { get; set; }
}