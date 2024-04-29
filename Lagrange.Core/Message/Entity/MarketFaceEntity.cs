namespace Lagrange.Core.Message.Entity;

using System.Collections.Generic;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;

[MessageElement(typeof(MarketFace))]
public class MarketFaceEntity : IMessageEntity
{
    public string FaceId { get; }

    public int TabId { get; }

    public string Key { get; }

    public string Summary { get; }

    public MarketFaceEntity() : this(string.Empty, default, string.Empty, string.Empty) { }

    public MarketFaceEntity(string faceId, int tabId, string key, string summary)
    {
        FaceId = faceId;
        TabId = tabId;
        Key = key;
        Summary = summary;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                MarketFace = new()
                {
                    Summary = Summary,
                    ItemType = 6,
                    Info = 1,
                    FaceId = FaceId.UnHex(),
                    TabId = TabId,
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
        if (elem.MarketFace == null) return null;

        return new MarketFaceEntity(
            elem.MarketFace.FaceId.Hex(true),
            elem.MarketFace.TabId,
            elem.MarketFace.Key,
            elem.MarketFace.Summary
        );
    }

    public string ToPreviewString()
    {
        return $"[{nameof(MarketFaceEntity)}: TabId: {TabId}; FaceId: {FaceId}; Key: {Key}; Summary: {Summary}]";
    }
}