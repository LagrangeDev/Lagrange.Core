using Lagrange.Core.Internal.Packets.Message.Element;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation;
using Lagrange.Core.Internal.Packets.Message.Element.Implementation.Extra;
using ProtoBuf;
using BitConverter = Lagrange.Core.Utility.Binary.BitConverter;

namespace Lagrange.Core.Message.Entity;

[MessageElement(typeof(Text))]
public class MentionEntity : IMessageEntity
{
    public uint Uin { get; set; }
    
    public string Uid { get; set; }
    
    public string? Name { get; set; }
    
    public MentionEntity()
    {
        Uin = 0;
        Uid = "";
        Name = "";
    }
    
    /// <summary>
    /// Set target to 0 to mention everyone
    /// </summary>
    public MentionEntity(string? name, uint target = 0)
    {
        Uin = target;
        Uid = ""; // automatically resolved by MessagingLogic.cs
        Name = name;
    }

    IEnumerable<Elem> IMessageEntity.PackElement()
    {
        var reserve = new MentionExtra
        {
            Type = Uin == 0 ? 1 : 2,
            Uin = 0,
            Field5 = 0,
            Uid = Uid,
        };
        using var stream = new MemoryStream();
        Serializer.Serialize(stream, reserve);
        
        return new Elem[]
        {
            new()
            {
                Text = new Text
                {
                    Str = Name,
                    PbReserve = stream.ToArray()
                }
            }
        };
    }
    
    IMessageEntity? IMessageEntity.UnpackElement(Elem elems)
    {
        if (elems.Text is { Str: not null, Attr6Buf: { } attr } && attr.Length >= 11)
        {
            var uin = BitConverter.ToUInt32(attr.AsSpan()[7..11], false);

            // 如果 PbReserve 不为空，执行特殊处理；否则处理默认逻辑
            if (elems.Text.PbReserve is not null)  // 狗日的TX，Markdown消息长文本时不提供PbReserve
            {
                // PbReserve 存在时的处理
                return new MentionEntity
                {
                    Name = elems.Text.Str,
                    Uin = uin,
                    Uid = ""
                };
            }
            else
            {
                // PbReserve 为空时的处理
                return new MentionEntity
                {
                    Name = elems.Text.Str,
                    Uin = uin,
                    Uid = ""
                };
            }
        }

        return null;
    }

    public string ToPreviewString()
    {
        return $"[Mention]: {Name}({Uin})";
    }

    public string ToPreviewText() => $"{Name} ";
}
