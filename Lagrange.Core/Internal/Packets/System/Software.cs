using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.System;

[ProtoContract]
internal class Software
{
    [ProtoMember(2)] public string Ver { get; set; }
    
    [ProtoMember(3)] public string? Token { get; set; }
}