using Lagrange.Core.Internal.Packets.Message;

namespace Lagrange.Core.Message.Entities;

public class MarketFaceEntity : IMessageEntity
{
    public int EmojiPackageId { get; internal init; }

    public string EmojiId { get; internal init; } = string.Empty;

    public string Key { get; internal init; } = string.Empty;

    public string Summary { get; internal init; } = string.Empty;

    public string Url => $"https://gxh.vip.qq.com/club/item/parcel/item/{EmojiId[..2]}/{EmojiId}/raw300.gif";

    Elem[] IMessageEntity.Build() => throw new NotSupportedException();

    IMessageEntity? IMessageEntity.Parse(List<Elem> elements, Elem target)
    {
        if (target.MarketFace is not { FaceId.Length: > 0 } face) return null;

        return new MarketFaceEntity
        {
            EmojiPackageId = (int)face.TabId,
            EmojiId = Convert.ToHexStringLower(face.FaceId),
            Key = face.Key,
            Summary = face.Summary
        };
    }

    string IMessageEntity.ToPreviewString() => Summary;
}