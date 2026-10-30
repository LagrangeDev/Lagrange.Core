using System.Text;
using System.Text.Json.Nodes;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Compression;

namespace Lagrange.Core.Message.Entities;

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
    
    Elem[] IMessageEntity.Build()
    {
        var compressed = ZCompression.ZCompress(Encoding.UTF8.GetBytes(Payload));
        using var payload = new BinaryPacket(1 + compressed.Length);
        payload.Write<byte>(0x01);
        payload.Write(compressed);

        return new Elem[]
        {
            new()
            {
                LightAppElem = new LightAppElem
                {
                    BytesData = payload.ToArray()
                }
            }
        };
    }

    IMessageEntity? IMessageEntity.Parse(List<Elem> elems, Elem target)
    {
        if (target.LightAppElem is { } lightApp)
        {
            var payload = ZCompression.ZDecompress(lightApp.BytesData.Span.Slice(1), false);
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