using System.IO.Compression;
using System.Text.Json.Nodes;
using System.Xml;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Events.Message;
using Lagrange.Core.Internal.Packets.Message;
using Lagrange.Core.Internal.Packets.Service;
using Lagrange.Core.Utility;

namespace Lagrange.Core.Message.Entities;

public class MultiMsgEntity(string? resId) : IMessageEntity
{
    public List<BotMessage> Messages { get; } = [];
    
    public string? ResId { get; private set; } = resId;
    
    public string Title { get; private init; } = string.Empty;

    public List<string> Preview { get; private init; } = [];

    public string Summary { get; private init; } = string.Empty;

    public MultiMsgEntity(List<BotMessage> messages) : this(default(string))
    {
        Messages = messages;
    }
    
    public MultiMsgEntity() : this(default(string)) { }

    public async Task Preprocess(BotContext context, BotMessage message)
    {
        if (string.IsNullOrEmpty(ResId))
        {
            var result = await context.EventContext.SendEvent<LongMsgSendEventResp>(new LongMsgSendEventReq(message.Receiver, Messages));
            ResId = result.ResId;

        }
    }
    
    public async Task Postprocess(BotContext context, BotMessage message)
    {
        // nested forwards are fetched on demand
        if (string.IsNullOrEmpty(ResId) || message.IsForwarded) return;

        bool isGroup = message.Contact is BotGroupMember;
        var result = await context.EventContext.SendEvent<LongMsgRecvEventResp>(new LongMsgRecvEventReq(isGroup, ResId));
        Messages.Clear();
        Messages.AddRange(result.Messages);
    }
    
    string IMessageEntity.ToPreviewString() => "[聊天记录]";

    Elem[] IMessageEntity.Build()
    {
        if (string.IsNullOrEmpty(ResId)) return [];
        
        int count = Math.Clamp(Messages.Count, 0, 4);
        string guid = Guid.NewGuid().ToString();
        var extra = new JsonObject { { "filename", guid }, { "tsum", count } };
        var news = new JsonArray(Messages[..count].Select(x => new JsonObject { { "text", $"{x.Contact.Nickname}: {string.Join(' ', x.Entities.Select(e => e.ToPreviewString()))}" } }).Cast<JsonNode>().ToArray());
        var detail = new JsonObject
        {
            { "news", news },
            { "resid", ResId },
            { "source", "聊天记录" },
            { "summary", $"查看{count}条转发消息" },
            { "uniseq", guid }
        };

        var lightApp = new LightApp
        {
            App = "com.tencent.multimsg",
            Config = new Config
            {
                Autosize = 1,
                Forward = 1,
                Round = 1,
                Type = "normal",
                Width = 300
            },
            Meta = new JsonObject { { "detail", detail } },
            Desc = "[聊天记录]",
            Extra = JsonHelper.Serialize(extra),
            Prompt = "[聊天记录]",
            Ver = "0.0.0.5",
            View = "contact"
        };
        
        var data = JsonHelper.SerializeToUtf8Bytes(lightApp).Span;
        using var dest = new MemoryStream();
        dest.WriteByte(0x01);
        using var zlib = new ZLibStream(dest, CompressionLevel.Optimal, true);
        zlib.Write(data);
        zlib.Close();

        return [new Elem { LightAppElem = new LightAppElem { BytesData = dest.ToArray() } }];
    }

    IMessageEntity? IMessageEntity.Parse(List<Elem> elements, Elem target)
    {
        if (target.RichMsg is { ServiceId: 35 } richMsg)
        {
            using var source = new MemoryStream();
            using var dest = new MemoryStream();
            using var inflate = new DeflateStream(source, CompressionMode.Decompress);
            source.Write(richMsg.BytesTemplate1.Span[3..^4]);
            source.Seek(0, SeekOrigin.Begin);
            
            inflate.CopyTo(dest);
            dest.Seek(0, SeekOrigin.Begin);
            
            using var xmlReader = XmlReader.Create(dest);
            xmlReader.Read();
            
            var doc = new XmlDocument();
            doc.Load(xmlReader);

            // the first title is the source, the rest are the preview lines
            var titles = doc.SelectNodes("/msg/item/title")?.Cast<XmlNode>().Select(x => x.InnerText).ToList() ?? [];
            return new MultiMsgEntity(doc["msg"]?.Attributes["m_resid"]?.Value ?? string.Empty)
            {
                Title = titles.FirstOrDefault() ?? string.Empty,
                Preview = titles.Skip(1).ToList(),
                Summary = doc.SelectSingleNode("/msg/item/summary")?.InnerText ?? string.Empty
            };
        }

        return null;
    }

    private static byte[] Adler32(ReadOnlySpan<byte> data)
    {
        uint a = 1, b = 0;
        foreach (byte t in data)
        {
            a = (a + t) % 65521;
            b = (b + a) % 65521;
        }
        return BitConverter.GetBytes((a << 16) | b);
    }
}