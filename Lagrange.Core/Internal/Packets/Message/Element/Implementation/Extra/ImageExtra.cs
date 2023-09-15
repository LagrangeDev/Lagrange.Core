using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// Used in conjunction with <see cref="NotOnlineImage"/>, should be created in a dedicated <see cref="Elem"/> and put in Leaf 53 <see cref="CommonElem"/>
/// <para>NTQQ Exclusive</para>
/// </summary>
[ProtoContract]
internal class ImageExtra
{
    [ProtoMember(85)] public uint Field85 { get; set; } // 1
}