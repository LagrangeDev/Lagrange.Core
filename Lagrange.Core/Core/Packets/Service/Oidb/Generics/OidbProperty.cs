using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Service.Oidb.Generics;

[ProtoContract]
internal class OidbProperty
{
    [ProtoMember(1)] public string Key { get; set; }
    
    [ProtoMember(2)] public byte[] Value { get; set; }
}