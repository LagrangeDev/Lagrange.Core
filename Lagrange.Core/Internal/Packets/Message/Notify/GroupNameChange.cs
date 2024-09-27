using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Notify;

[ProtoContract]
internal class GroupNameChange
{
    [ProtoMember(2)] public string Name { get; set; }
}