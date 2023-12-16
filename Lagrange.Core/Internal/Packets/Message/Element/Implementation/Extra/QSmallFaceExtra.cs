using ProtoBuf;


#pragma warning disable CS8618
namespace Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;

[ProtoContract]
internal class QSmallFaceExtra
{
    [ProtoMember(1)] public uint FaceId { get; set; }
    
    [ProtoMember(2)] public string Preview { get; set; }
    
    [ProtoMember(3)] public string Preview2 { get; set; }
}