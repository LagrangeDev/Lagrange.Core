using System.Text;
using System.Text.Json.Nodes;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(LightAppElem))]
public class LightAppEntity : IMessageEntity
{
    public string AppName { get; set; } = string.Empty;
    
    public string Payload { get; set; } = string.Empty;
    
    public LightAppEntity() { }

    public LightAppEntity(string payload)
    {
        Payload = payload;
        string? app = JsonNode.Parse(payload)?["app"]?.ToString();
        if (app != null) AppName = app;
    }
    
    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        using var payload = new BinaryPacket()
            .WriteByte(0x01)
            .WriteBytes(ZCompression.ZCompress(Encoding.UTF8.GetBytes(Payload)));

        return new Elem[]
        {
            new()
            {
                LightAppElem = new LightAppElem
                {
                    Data = payload.ToArray(),
                    MsgResid = null
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.LightAppElem is { } lightApp)
        {
            var payload = ZCompression.ZDecompress(lightApp.Data.AsSpan(1), false);
            string json = Encoding.UTF8.GetString(payload);
            string? app = JsonNode.Parse(json)?["app"]?.ToString();

            if (app != null)
            {
                return new LightAppEntity
                {
                    AppName = app,
                    Payload = json
                };
            }
        }
        
        return null;
    }

    public string ToPreviewString()
    {
        return $"[{nameof(LightAppEntity)}: {AppName}]";
    }
}