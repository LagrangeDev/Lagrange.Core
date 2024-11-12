using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class RegisterInfoResponse
{
    [ProtoMember(2)] public string? Message { get; set; }
    
    [ProtoMember(3)] public uint Timestamp { get; set; }
    
    [ProtoMember(4)] public int Field4 { get; set; }
    
    [ProtoMember(5)] public int Field5 { get; set; }
}