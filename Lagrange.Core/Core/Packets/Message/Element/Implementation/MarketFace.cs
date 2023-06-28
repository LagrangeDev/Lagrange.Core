using ProtoBuf;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8618

namespace Lagrange.Core.Core.Packets.Message.Element.Implementation;

[ProtoContract]
internal class MarketFace
{
    [ProtoMember(1)] public byte[] FaceName { get; set; }
    
    [ProtoMember(2)] public int ItemType { get; set; }
    
    [ProtoMember(3)] public int FaceInfo { get; set; }
    
    [ProtoMember(4)] public byte[] FaceId { get; set; }
    
    [ProtoMember(5)] public int TabId { get; set; }
    
    [ProtoMember(6)] public int SubType { get; set; }
    
    [ProtoMember(7)] public byte[] Key { get; set; }
    
    [ProtoMember(8)] public byte[] Param { get; set; }
    
    [ProtoMember(9)] public int MediaType { get; set; }
    
    [ProtoMember(10)] public int ImageWidth { get; set; }
    
    [ProtoMember(11)] public int ImageHeight { get; set; }
    
    [ProtoMember(12)] public byte[] Mobileparam { get; set; }
    
    [ProtoMember(13)] public byte[] PbReserve { get; set; }
}