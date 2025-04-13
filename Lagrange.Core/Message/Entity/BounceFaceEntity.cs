using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class BounceFaceEntity : IMessageEntity
{
    public uint FaceId { get; set; }

    public uint Count { get; set; }

    public string Name { get; set; }

    public bool ShouldAddPreviewText { get; set; }

    public BounceFaceEntity() : this(0, 0, string.Empty) {}

    public BounceFaceEntity(SysFaceEntry face, uint count, bool shouldAddPreviewText = true)
    {
        if (!face.AniStickerId.HasValue)
            throw new ArgumentException("Face does not have a sticker ID", nameof(face));

        FaceId = (uint)face.AniStickerId.Value;
        Count = count;
        Name = face.QDes ?? string.Empty;
        ShouldAddPreviewText = shouldAddPreviewText;

        // Because the name is used as a preview text, it should not start with '/'
        // But the QDes of the face may start with '/', so remove it
        if (Name.StartsWith('/'))
            Name = Name[1..];
    }

    public BounceFaceEntity(uint faceId, uint count, string? name = null, bool shouldAddPreviewText = true)
    {
        FaceId = faceId;
        Count = count;

        // If the name is null, it will be assigned in MessagingLogic.ResolveOutgoingChain
        Name = name!;
        ShouldAddPreviewText = shouldAddPreviewText;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        byte[] pbElem;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, new QBounceFaceExtra
            {
                Field1 = 13,
                Face = new QSmallFaceExtra
                {
                    FaceId = FaceId,
                    Text = Name,
                    CompatText = Name
                },
                Count = Count,
                Name = Name
            });
            pbElem = ms.ToArray();
        }

        var common = new CommonElem
        {
            ServiceType = 23,
            PbElem = pbElem,
            BusinessType = 13
        };

        if (!ShouldAddPreviewText)
            return new Elem[] { new() { CommonElem = common } };

        byte[] textFallbackPb;
        using (var ms = new MemoryStream())
        {
            Serializer.Serialize(ms, new QBounceFaceExtra.FallbackPreviewTextPb { Text = $"[{Name}]请使用最新版手机QQ体验新功能。" });
            textFallbackPb = ms.ToArray();
        }
        return new Elem[]
        {
            new() { CommonElem = common },
            new()
            {
                Text = new Text
                {
                    Str = ToPreviewText(),
                    PbReserve = textFallbackPb
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem is not { ServiceType: 23 } common)
            return null;

        var extra = Serializer.Deserialize<QBounceFaceExtra>(common.PbElem.AsSpan());
        return new BounceFaceEntity(extra.Face.FaceId, extra.Count, extra.Name);
    }

    public string ToPreviewString() => $"[BounceFace | Name: {Name}({FaceId}) | Count: {Count}]";

    public string ToPreviewText() => $"[{Name}]x{Count}";
}