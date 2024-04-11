using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Notify;

#pragma warning disable CS8618

[ProtoContract]
internal class GroupUniqueTitleChange
{
    [ProtoMember(2)] public string Wording { get; set; }

    [ProtoMember(3)] public uint Field3 { get; set; }

    [ProtoMember(5)] public uint TargetUin { get; set; }
}
