using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation;

[ProtoContract]
internal class Marketface
{
    [ProtoMember(1)] public string Summary { get; set; }

    [ProtoMember(2)] public int ItemType { get; set; }

    [ProtoMember(3)] public int Info { get; set; }

    [ProtoMember(4)] public byte[] FaceId { get; set; }

    [ProtoMember(5)] public int TabId { get; set; }

    [ProtoMember(6)] public int SubType { get; set; }

    [ProtoMember(7)] public string Key { get; set; }

    [ProtoMember(10)] public int Width { get; set; }

    [ProtoMember(11)] public int Height { get; set; }

    [ProtoMember(13)] public MarketfaceReserve PbReserve { get; set; }
}

[ProtoContract]
internal class MarketfaceReserve
{
    [ProtoMember(8)] public int Field8 { get; set; }

}