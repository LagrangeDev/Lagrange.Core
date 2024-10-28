using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(Face)), MessageElement(typeof(CommonElem))]
public class FaceEntity : IMessageEntity
{
    public ushort FaceId { get; set; }

    public bool IsLargeFace { get; set; }
    
    public SysFaceEntry? SysFaceEntry { get; set; }

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
            var qBigFace = new QBigFaceExtra
            {
                AniStickerPackId = SysFaceEntry?.AniStickerPackId.ToString() ?? "1",
                AniStickerId = SysFaceEntry?.AniStickerId.ToString() ?? "8",
                FaceId = FaceId,
                Field4 = 1,
                AniStickerType = SysFaceEntry?.AniStickerType ?? 1,
                Field6 = "",
                Preview = SysFaceEntry?.QDes ?? "",
                Field9 = 1
            };
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, qBigFace);
            return new Elem[]
            {
                new()
                {
                    CommonElem = new CommonElem
                    {
                        ServiceType = 37,
                        PbElem = stream.ToArray(),
                        BusinessType = (uint)(SysFaceEntry?.AniStickerType ?? 1)
                    }
                }
            };
        }
        
        if (FaceId >= 260)
        {
            var qSmallFace = new QSmallFaceExtra
            {
                FaceId = FaceId,
                Text = SysFaceEntry?.QDes ?? "",
                CompatText = SysFaceEntry?.QDes ?? ""
            };
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, qSmallFace);
            return new Elem[]
            {
                new()
                {
                    CommonElem = new CommonElem
                    {
                        ServiceType = 33,
                        PbElem = stream.ToArray(),
                        BusinessType = (uint)(SysFaceEntry?.AniStickerType ?? 1)
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
            var qFace = Serializer.Deserialize<QBigFaceExtra>(common.PbElem.AsSpan());
            
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