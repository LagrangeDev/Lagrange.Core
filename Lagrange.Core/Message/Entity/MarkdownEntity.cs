using System.Text.Json;
using System.Text.Json.Serialization;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Extension;
using ProtoBuf;

namespace Lagrange.Core.Message.Entity;

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

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem) => null;

    public string ToPreviewString() => throw new NotImplementedException();
}

[ProtoContract]
public class MarkdownData
{
    [JsonPropertyName("content")] [ProtoMember(1)]
    public string Content { get; set; } = string.Empty;
}