using ProtoBuf;

namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

/// <summary>
/// Constructed at <see cref="CommonElem"/>, Service Type 33, Big face
/// </summary>
[ProtoContract]
internal class QBigFaceExtra
{
    [ProtoMember(1)] public string? AniStickerPackId { get; set; }
    
    [ProtoMember(2)] public string? AniStickerId { get; set; }
    
    [ProtoMember(3)] public int? FaceId { get; set; } // 318
    
    [ProtoMember(4)] public int? Field4 { get; set; }
    
    [ProtoMember(5)] public int? AniStickerType { get; set; }
    
    [ProtoMember(6)] public string? Field6 { get; set; }
    
    [ProtoMember(7)] public string? Preview { get; set; }
    
    [ProtoMember(9)] public int? Field9 { get; set; }
}