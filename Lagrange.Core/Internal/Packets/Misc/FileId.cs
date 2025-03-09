using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Misc;

[ProtoContract]
public class FileId
{
    [ProtoMember(4)]
    public ulong AppId { get; set; }

    [ProtoMember(10)]
    public uint Ttl { get; set; }
}