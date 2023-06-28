using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.System;

[ProtoContract]
internal class Software
{
    [ProtoMember(2) ]public string Ver { get; set; }
}