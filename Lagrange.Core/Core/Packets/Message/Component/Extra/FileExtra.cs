using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Component.Extra;

[ProtoContract]
internal class FileExtra
{
    [ProtoMember(1)] public NotOnlineFile? File { get; set; }
}