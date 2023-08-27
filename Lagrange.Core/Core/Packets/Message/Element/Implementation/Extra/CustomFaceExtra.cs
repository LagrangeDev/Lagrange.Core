using ProtoBuf;

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class CustomFaceExtra
{
    public int? Field1 { get; set; }
}