using ProtoBuf;
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class LightAppElem
{
    [ProtoMember(1)] public byte[] Data { get; set; }

    [ProtoMember(2)] public byte[]? MsgResid { get; set; }
}