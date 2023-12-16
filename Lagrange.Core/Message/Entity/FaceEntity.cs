using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(Face)), MessageElement(typeof(CommonElem))]
public class FaceEntity : IMessageEntity
{
    public ushort FaceId { get; }
    
    public bool IsLargeFace { get; }
    
    public FaceEntity() { }

    public FaceEntity(ushort faceId, bool isLargeFace)
    {
        FaceId = faceId;
        IsLargeFace = isLargeFace;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        if (IsLargeFace)
        {
            var qFace = new QFaceExtra
            {
                Field1 = "1",
                Field2 = "8",
                FaceId = FaceId,
                Field4 = 1,
                Field5 = 1,
                Field6 = "",
                Preview = "",
                Field9 = 1
            };
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, qFace);

            return new Elem[]
            {
                new()
                {
                    CommonElem = new CommonElem
                    {
                        ServiceType = 37,
                        PbElem = stream.ToArray(),
                        BusinessType = 1
                    }
                }
            };
        }
        
        return new Elem[] { new() { Face = new Face { Index = FaceId } } };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.Face is { Old: not null } face)
        {
            ushort? faceId = (ushort?)face.Index;
            if (faceId != null) return new FaceEntity((ushort)faceId, false);
        }

        if (elems.CommonElem is { ServiceType:37, PbElem: not null } common)
        {
            var qFace = Serializer.Deserialize<QFaceExtra>(common.PbElem.AsSpan());
            
            ushort? faceId = (ushort?)qFace.FaceId;
            if (faceId != null) return new FaceEntity((ushort)faceId, true);
        }

        if (elems.CommonElem is { ServiceType: 33, PbElem: not null } append)
        {
            var qSmallFace = Serializer.Deserialize<QSmallFaceExtra>(append.PbElem.AsSpan());
            return new FaceEntity((ushort)qSmallFace.FaceId, false);
        }
        
        return null;
    }

    public string ToPreviewString() => $"[Face][{(IsLargeFace ? "Large" : "Small")}]: {FaceId}";
}