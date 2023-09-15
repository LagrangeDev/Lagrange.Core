using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class FileExtra
{
    [ProtoMember(1)] public NotOnlineFile? File { get; set; }
}