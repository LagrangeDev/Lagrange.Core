namespace Lagrange.Core.Message.Entity;

using System.Collections.Generic;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;

[MessageElement(typeof(Marketface))]
public class MarketfaceEntity : IMessageEntity
{
    public string EmojiId { get; set; }

    public int EmojiPackageId { get; set; }

    public string Key { get; set; }

    public string Summary { get; set; }

    public MarketfaceEntity() : this(string.Empty, default, string.Empty, string.Empty) { }

    public MarketfaceEntity(string faceId, int tabId, string key, string summary)
    {
        EmojiId = faceId;
        EmojiPackageId = tabId;
        Key = key;
        Summary = summary;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                Marketface = new()
                {
                    Summary = Summary,
                    ItemType = 6,
                    Info = 1,
                    FaceId = EmojiId.UnHex(),
                    TabId = EmojiPackageId,
                    SubType = 3,
                    Key = Key,
                    // Param = 
                    // MediaType
                    Width = 300,
                    Height = 300,
                    PbReserve = new()
                    {
                        // Size = 
                        Field8 = 1
                    }
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.Marketface == null) return null;

        return new MarketfaceEntity(
            elem.Marketface.FaceId.Hex(true),
            elem.Marketface.TabId,
            elem.Marketface.Key,
            elem.Marketface.Summary
        );
    }

    public string ToPreviewString()
    {
        return $"[{nameof(MarketfaceEntity)}: TabId: {EmojiPackageId}; FaceId: {EmojiId}; Key: {Key}; Summary: {Summary}]";
    }
}