using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Utility.Binary;
using Lagrange.Core.Utility.Binary.Compression;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(RichMsg))]
public class MultiMsgEntity : IMessageEntity
{
    private static readonly XmlSerializer Serializer = new(typeof(MultiMessage));

    internal string? ResId { get; set; }

    public uint? GroupUin { get; set; }

    public List<MessageChain> Chains { get; }

    internal MultiMsgEntity() => Chains = new List<MessageChain>();

    internal MultiMsgEntity(string resId)
    {
        ResId = resId;
        Chains = new List<MessageChain>();
    }

    internal MultiMsgEntity(uint? groupUin, List<MessageChain> chains)
    {
        GroupUin = groupUin;
        Chains = chains;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        int count = Math.Clamp(Chains.Count, 0, 4);
        string fileId = Guid.NewGuid().ToString();
        var extra = new MultiMsgLightAppExtra
        {
            FileName = fileId,
            Sum = count
        };

        var json = new MultiMsgLightApp
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
            Desc = "[聊天记录]",
            Extra = JsonSerializer.Serialize(extra),
            Meta = new Meta
            {
                Detail = new Detail
                {
                    News = new List<News>(),
                    Resid = ResId ?? "",
                    Source = "聊天记录",
                    Summary = $"查看{Chains.Count}条转发消息",
                    UniSeq = fileId
                }
            },
            Prompt = "[聊天记录]",
            Ver = "0.0.0.5",
            View = "contact"
        };

        if (!Chains.Select(x => x.GetEntity<TextEntity>()).Any())
        {
            json.Meta.Detail.News.Add(new News { Text = "[This message is send from Lagrange.Core]" });
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                var chain = Chains[i];
                var member = chain.GroupMemberInfo;
                var friend = chain.FriendInfo;
                string text = $"{member?.MemberCard ?? member?.MemberName ?? friend?.Nickname}: {chain.ToPreviewText()}";
                json.Meta.Detail.News.Add(new News { Text = text });
            }
        }

        using var payload = new BinaryPacket()
            .WriteByte(0x01)
            .WriteBytes(ZCompression.ZCompress(JsonSerializer.SerializeToUtf8Bytes(json)));

        return new Elem[]
        {
            new() { LightAppElem = new LightAppElem { Data = payload.ToArray() } },
        };
    }

    IMessageEntity? IMessageEntity.UnpackElement(Elem elem)
    {
        if (elem.RichMsg is { ServiceId: 35, Template1: not null } richMsg)
        {
            var xml = ZCompression.ZDecompress(richMsg.Template1[1..]);
            if ((MultiMessage?)Serializer.Deserialize(new MemoryStream(xml)) is { } xmlEntity)
            {
                return new MultiMsgEntity(xmlEntity.ResId);
            }
        }

        return null;
    }


    public string ToPreviewString() => $"[MultiMsgEntity] {Chains.Count} chains";

    public string ToPreviewText() => "[聊天记录]";

    #region Json

    [Serializable]
    private class MultiMsgLightApp
    {
        [JsonPropertyName("app")] public string App { get; set; }

        [JsonPropertyName("config")] public Config Config { get; set; }

        [JsonPropertyName("desc")] public string Desc { get; set; }

        [JsonPropertyName("extra")] public string Extra { get; set; }

        [JsonPropertyName("meta")] public Meta Meta { get; set; }

        [JsonPropertyName("prompt")] public string Prompt { get; set; }

        [JsonPropertyName("ver")] public string Ver { get; set; }

        [JsonPropertyName("view")] public string View { get; set; }
    }

    private class MultiMsgLightAppExtra
    {
        [JsonPropertyName("filename")] public string FileName { get; set; }

        [JsonPropertyName("tsum")] public int Sum { get; set; }
    }

    [Serializable]
    private class Config
    {
        [JsonPropertyName("autosize")] public long Autosize { get; set; }

        [JsonPropertyName("forward")] public long Forward { get; set; }

        [JsonPropertyName("round")] public long Round { get; set; }

        [JsonPropertyName("type")] public string Type { get; set; }

        [JsonPropertyName("width")] public long Width { get; set; }
    }

    [Serializable]
    private class Meta
    {
        [JsonPropertyName("detail")] public Detail Detail { get; set; }
    }

    [Serializable]
    private class Detail
    {
        [JsonPropertyName("news")] public List<News> News { get; set; }

        [JsonPropertyName("resid")] public string Resid { get; set; }

        [JsonPropertyName("source")] public string Source { get; set; }

        [JsonPropertyName("summary")] public string Summary { get; set; }

        [JsonPropertyName("uniseq")] public string UniSeq { get; set; }
    }

    [Serializable]
    private class News
    {
        [JsonPropertyName("text")] public string Text { get; set; }
    }

    #endregion

    #region Xml

    [Serializable]
    [XmlRoot("msg")]
    public class MultiMessage
    {
        [XmlAttribute("serviceID")] public uint ServiceId { get; set; } // 35

        [XmlAttribute("templateID")] public int TemplateId { get; set; } // 1

        [XmlAttribute("action")] public string Action { get; set; } = "viewMultiMsg";

        [XmlAttribute("brief")] public string Brief { get; set; } = "[消息记录]";

        [XmlAttribute("m_fileName")] public string FileName { get; set; } = Guid.NewGuid().ToString("N");

        [XmlAttribute("m_resid")] public string ResId { get; set; } = Guid.NewGuid().ToString("N");

        [XmlAttribute("tSum")] public int Total { get; set; } // 2

        [XmlAttribute("flag")] public int Flag { get; set; } // 3

        [XmlElement("item")] public MultiItem Item { get; set; } = new();

        [XmlElement("source")] public MultiSource Source { get; set; } = new();
    }

    [Serializable]
    public class MultiItem
    {
        [XmlAttribute("layout")] public int Layout { get; set; } // 1

        [XmlElement("title")] public List<MultiTitle> Title { get; set; } = new();

        [XmlElement("summary")] public MultiSummary Summary { get; set; } = new();
    }

    [Serializable]
    public class MultiTitle
    {
        [XmlAttribute("color")] public string Color { get; set; }

        [XmlAttribute("size")] public int Size { get; set; }

        [XmlText] public string Text { get; set; }

        public MultiTitle(string color, int size, string text)
        {
            Color = color;
            Size = size;
            Text = text;
        }

        public MultiTitle() { }
    }

    [Serializable]
    public class MultiSummary
    {
        [XmlAttribute("color")] public string Color { get; set; } = "#000000";

        [XmlText] public string Text { get; set; } = string.Empty;
    }

    [Serializable]
    public class MultiSource
    {
        [XmlAttribute("name")] public string Name { get; set; } = string.Empty;
    }

    #endregion
}
