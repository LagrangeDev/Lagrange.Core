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

    public MarketFaceEntity() : this(string.Empty, default, string.Empty) { }

    public MarketFaceEntity(string faceId, int tabId, string key)
    {
        FaceId = faceId;
        TabId = tabId;
        Key = key;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                MarketFace = new()
                {
                    Name = "[\u5546\u57ce\u8868\u60c5]",
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

        return new MarketFaceEntity(elem.MarketFace.FaceId.Hex(), elem.MarketFace.TabId, elem.MarketFace.Key);
    }

    public string ToPreviewString() => $"[{nameof(MarketFaceEntity)}: {TabId} - {FaceId} - {Key}]";
}