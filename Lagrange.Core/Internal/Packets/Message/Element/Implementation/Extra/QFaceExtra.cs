using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// Constructed at <see cref="CommonElem"/>, Service Type 33, Big face
/// </summary>
[ProtoContract]
internal class QFaceExtra
{
    [ProtoMember(1)] public string? Field1 { get; set; }
    
    [ProtoMember(2)] public string? AniStickerId { get; set; }
    
    [ProtoMember(3)] public int? FaceId { get; set; }
    
    [ProtoMember(4)] public int? AniStickerPackId { get; set; }
    
    [ProtoMember(5)] public int? Field5 { get; set; }  // maybe AniStickerType
    
    [ProtoMember(6)] public string? Field6 { get; set; }
    
    [ProtoMember(7)] public string? Preview { get; set; }
    
    [ProtoMember(9)] public int? Field9 { get; set; }
}