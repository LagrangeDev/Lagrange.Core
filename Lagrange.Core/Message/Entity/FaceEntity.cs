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
                AniStickerPackId = "1",
                AniStickerId = _stickerIds.ContainsKey(FaceId) ? _stickerIds[FaceId] : "8",
                FaceId = FaceId,
                Field4 = 1,
                AniStickerType = FaceId == 114 ? 2 : 1,
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
                        BusinessType = FaceId==114?(uint)2:1,
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

        if (elems.CommonElem is { ServiceType: 37, PbElem: not null } common)
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

    private static Dictionary<int, string> _stickerIds = new()
    {
        { 5, "16" },
        { 53, "17" },
        { 114, "13" },
        { 137, "18" },
        { 311, "1" },
        { 312, "2" },
        { 313, "3" },
        { 314, "4" },
        { 315, "5" },
        { 316, "6" },
        { 317, "7" },
        { 318, "8" },
        { 319, "9" },
        { 320, "10" },
        { 321, "11" },
        { 324, "12" },
        { 325, "14" },
        { 326, "15" },
        { 333, "19" },
        { 337, "22" },
        { 338, "20" },
        { 339, "21" },
        { 340, "23" },
        { 341, "24" },
        { 342, "26" },
        { 343, "27" },
        { 344, "28" },
        { 345, "29" },
        { 346, "25" },
    };
}