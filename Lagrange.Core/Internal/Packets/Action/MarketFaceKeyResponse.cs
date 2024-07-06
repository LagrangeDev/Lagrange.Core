using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class MarketFaceKeyResponse
{
    [ProtoMember(5)] public MarketFaceKeyInfo Info { get; set; }
}

[ProtoContract]
internal class MarketFaceKeyInfo
{
    [ProtoMember(1)] public List<string> Keys { get; set; }
}