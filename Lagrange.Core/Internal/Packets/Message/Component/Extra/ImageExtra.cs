using ProtoBuf;

#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Component.Extra;

[ProtoContract]
internal class ImageExtraKey
{
    [ProtoMember(30)] public string RKey { get; set; }
}