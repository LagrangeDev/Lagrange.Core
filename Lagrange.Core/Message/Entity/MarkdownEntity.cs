using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(CommonElem))]
public class MarkdownEntity : IMessageEntity
{
    public MarkdownData Data { get; set; }
    
    internal MarkdownEntity() => Data = new MarkdownData();
    
    public MarkdownEntity(MarkdownData data) => Data = data;
    
    public MarkdownEntity(string data) => Data = JsonSerializer.Deserialize<MarkdownData>(data) ?? throw new Exception();
    
    IEnumerable<Elem> IMessageEntity.PackElement() => new Elem[]
    {
        new()
        {
            CommonElem = new CommonElem
            {
                ServiceType = 45,
                PbElem = Data.Serialize().ToArray(),
                BusinessType = 1
            }
        }
    };

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.CommonElem?.ServiceType != 45 || elem.CommonElem?.BusinessType != 1)
            return null;

        return new MarkdownEntity(Serializer.Deserialize<MarkdownData>(elem.CommonElem.PbElem.AsSpan()));
    }

    public string ToPreviewString()
    {
        return $"[Markdown] {Data.Content}";
    }
}

[ProtoContract]
public class MarkdownData
{
    [JsonPropertyName("content")] [ProtoMember(1)]
    public string Content { get; set; } = string.Empty;
}