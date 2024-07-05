using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Action;

#pragma warning disable CS8618

[ProtoContract]
internal class MarketFaceKeyRequest
{
    [ProtoMember(1)] public uint Field1 { get; set; }

    [ProtoMember(5)] public MarketFaceInfo Info { get; set; }
}

[ProtoContract]
internal class MarketFaceInfo
{
    [ProtoMember(3)] public List<string> FaceIds { get; set; }
}