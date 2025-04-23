using System.Text;
using System.Text.Json.Nodes;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary.Compression;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(RichMsg))]
public class JsonEntity : IMessageEntity
{
    public string Json { get; set; }
    
    public string ResId { get; set; }

    private static ReadOnlySpan<byte> Header => new byte[1] { 0x01 };
    
    public JsonEntity()
    {
        Json = "";
        ResId = "";
    }
    
    public JsonEntity(string json, string resId = "")
    {
        Json = json;
        ResId = resId;
    }
    
    public JsonEntity(JsonNode json, string resId = "")
    {
        Json = json.ToJsonString();
        ResId = resId;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        return new Elem[]
        {
            new()
            {
                Text = new Text { Str = ResId }
            },
            new()
            {
                RichMsg = new RichMsg
                {
                    ServiceId = 1,
                    Template1 = ZCompression.ZCompress(Json, Header),
                }
            }
        };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.RichMsg is { ServiceId: 1, Template1: not null } richMsg)
        {
            var json = ZCompression.ZDecompress(richMsg.Template1.AsSpan(1));
            return new XmlEntity(Encoding.UTF8.GetString(json));
        }

        return null;
    }

    public string ToPreviewString()
    {
        throw new NotImplementedException();
    }
}