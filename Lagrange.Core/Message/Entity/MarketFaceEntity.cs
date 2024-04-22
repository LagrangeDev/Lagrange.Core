namespace Lagrange.Core.Message.Entity;

using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;

[MessageElement(typeof(MarketFace))]
public class MarketFaceEntity : IMessageEntity
{
    public string FaceId { get; } = string.Empty;

    public int TabId { get; }

    public MarketFaceEntity() { }

    public MarketFaceEntity(string faceId, int tabId)
    {
        FaceId = faceId;
        TabId = tabId;
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
                    Key = "0000000000000000",
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

        return new MarketFaceEntity(elem.MarketFace.FaceId.Hex(), elem.MarketFace.TabId);
    }

    public string ToPreviewString() => $"[{nameof(MarketFaceEntity)}: {TabId} - {FaceId}]";
}