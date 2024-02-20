using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
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
        var xmlEntity = new MultiMessage
        {
            ServiceId = 35,
            TemplateId = 1,
            Total = Chains.Count,
            Flag = 3,
            ResId = ResId ?? ""
        };
        
        var item = xmlEntity.Item;
        item.Layout = 1;
        item.Title.Add(new MultiTitle("#000000", 34, "群聊的聊天记录"));
        for (int i = 0; i < Math.Min(Chains.Count, 3); i++)
        {
            var chain = Chains[i];
            string? name = chain.FriendInfo?.Nickname ?? chain.GroupMemberInfo?.MemberCard ?? chain.GroupMemberInfo?.MemberName;
            item.Title.Add(new MultiTitle("#777777", 26, $"{name}: {chain.GetEntity<TextEntity>()?.Text}"));
        }
        item.Summary.Color = "#808080";
        item.Summary.Text = $"查看{Chains.Count}条转发信息";
        
        xmlEntity.Source.Name = "群聊的聊天记录";
        
        var xml = new StringBuilder();
        var writer = XmlWriter.Create(xml, new XmlWriterSettings
        {
            Encoding = Encoding.UTF8,
            DoNotEscapeUriAttributes = true
        });
        Serializer.Serialize(writer, xmlEntity);
        writer.Dispose();
        
        return new Elem[]
        {
            new()
            { 
                RichMsg = new RichMsg
                {
                    ServiceId = 35,
                    Template1 = ZCompression.ZCompress(xml.ToString(), new byte[] { 0x01 })
                }
            },
            new() { GeneralFlags = new GeneralFlags { LongTextResId = ResId } }
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

    [Serializable]
    [XmlRoot("msg")]
    public class MultiMessage
    {
        [XmlAttribute("serviceID")] public uint ServiceId { get; set; } // 35

        [XmlAttribute("templateID")] public uint TemplateId { get; set; } // 1
        
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
}