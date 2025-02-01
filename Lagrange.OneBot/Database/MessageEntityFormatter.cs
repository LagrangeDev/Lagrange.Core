using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using MessagePack;
using MessagePack.Formatters;

namespace Lagrange.OneBot.Database;

public class MessageEntityFormatter : IMessagePackFormatter<IMessageEntity?>
{
    public IMessageEntity? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        return (MessageType)reader.ReadByte() switch
        {
            MessageType.BounceFace => MessagePackSerializer.Deserialize<BounceFaceEntity>(ref reader, options),
            MessageType.Face => MessagePackSerializer.Deserialize<FaceEntity>(ref reader, options),
            MessageType.File => MessagePackSerializer.Deserialize<FileEntity>(ref reader, options),
            MessageType.Forward => MessagePackSerializer.Deserialize<ForwardEntity>(ref reader, options),
            MessageType.GreyTip => MessagePackSerializer.Deserialize<GreyTipEntity>(ref reader, options),
            MessageType.GroupReaction => MessagePackSerializer.Deserialize<GroupReactionEntity>(ref reader, options),
            MessageType.Image => MessagePackSerializer.Deserialize<ImageEntity>(ref reader, options),
            MessageType.Json => MessagePackSerializer.Deserialize<JsonEntity>(ref reader, options),
            MessageType.Keyboard => MessagePackSerializer.Deserialize<KeyboardEntity>(ref reader, options),
            MessageType.LightApp => MessagePackSerializer.Deserialize<LightAppEntity>(ref reader, options),
            MessageType.LongMsg => MessagePackSerializer.Deserialize<LongMsgEntity>(ref reader, options),
            MessageType.Markdown => MessagePackSerializer.Deserialize<MarkdownEntity>(ref reader, options),
            MessageType.Marketface => MessagePackSerializer.Deserialize<MarketfaceEntity>(ref reader, options),
            MessageType.Mention => MessagePackSerializer.Deserialize<MentionEntity>(ref reader, options),
            MessageType.MultiMsg => MessagePackSerializer.Deserialize<MultiMsgEntity>(ref reader, options),
            MessageType.Poke => MessagePackSerializer.Deserialize<PokeEntity>(ref reader, options),
            MessageType.Record => MessagePackSerializer.Deserialize<RecordEntity>(ref reader, options),
            MessageType.SpecialPoke => MessagePackSerializer.Deserialize<SpecialPokeEntity>(ref reader, options),
            MessageType.Text => MessagePackSerializer.Deserialize<TextEntity>(ref reader, options),
            MessageType.Video => MessagePackSerializer.Deserialize<VideoEntity>(ref reader, options),
            MessageType.Xml => MessagePackSerializer.Deserialize<XmlEntity>(ref reader, options),
            _ => null,
        };
    }

    public void Serialize(ref MessagePackWriter writer, IMessageEntity? value, MessagePackSerializerOptions options)
    {
        switch (value)
        {
            case BounceFaceEntity:
            {
                writer.Write((byte)MessageType.BounceFace);
                MessagePackSerializer.Serialize(ref writer, (BounceFaceEntity)value, options);
                break;
            }
            case FaceEntity:
            {
                writer.Write((byte)MessageType.Face);
                MessagePackSerializer.Serialize(ref writer, (FaceEntity)value, options);
                break;
            }
            case FileEntity:
            {
                writer.Write((byte)MessageType.File);
                MessagePackSerializer.Serialize(ref writer, (FileEntity)value, options);
                break;
            }
            case ForwardEntity:
            {
                writer.Write((byte)MessageType.Forward);
                MessagePackSerializer.Serialize(ref writer, (ForwardEntity)value, options);
                break;
            }
            case GreyTipEntity:
            {
                writer.Write((byte)MessageType.GreyTip);
                MessagePackSerializer.Serialize(ref writer, (GreyTipEntity)value, options);
                break;
            }
            case GroupReactionEntity:
            {
                writer.Write((byte)MessageType.GroupReaction);
                MessagePackSerializer.Serialize(ref writer, (GroupReactionEntity)value, options);
                break;
            }
            case ImageEntity:
            {
                writer.Write((byte)MessageType.Image);
                MessagePackSerializer.Serialize(ref writer, (ImageEntity)value, options);
                break;
            }
            case JsonEntity:
            {
                writer.Write((byte)MessageType.Json);
                MessagePackSerializer.Serialize(ref writer, (JsonEntity)value, options);
                break;
            }
            case KeyboardEntity:
            {
                writer.Write((byte)MessageType.Keyboard);
                MessagePackSerializer.Serialize(ref writer, (KeyboardEntity)value, options);
                break;
            }
            case LightAppEntity:
            {
                writer.Write((byte)MessageType.LightApp);
                MessagePackSerializer.Serialize(ref writer, (LightAppEntity)value, options);
                break;
            }
            case LongMsgEntity:
            {
                writer.Write((byte)MessageType.LongMsg);
                MessagePackSerializer.Serialize(ref writer, (LongMsgEntity)value, options);
                break;
            }
            case MarkdownEntity:
            {
                writer.Write((byte)MessageType.Markdown);
                MessagePackSerializer.Serialize(ref writer, (MarkdownEntity)value, options);
                break;
            }
            case MarketfaceEntity:
            {
                writer.Write((byte)MessageType.Marketface);
                MessagePackSerializer.Serialize(ref writer, (MarketfaceEntity)value, options);
                break;
            }
            case MentionEntity:
            {
                writer.Write((byte)MessageType.Mention);
                MessagePackSerializer.Serialize(ref writer, (MentionEntity)value, options);
                break;
            }
            case MultiMsgEntity:
            {
                writer.Write((byte)MessageType.MultiMsg);
                MessagePackSerializer.Serialize(ref writer, (MultiMsgEntity)value, options);
                break;
            }
            case PokeEntity:
            {
                writer.Write((byte)MessageType.Poke);
                MessagePackSerializer.Serialize(ref writer, (PokeEntity)value, options);
                break;
            }
            case RecordEntity:
            {
                writer.Write((byte)MessageType.Record);
                MessagePackSerializer.Serialize(ref writer, (RecordEntity)value, options);
                break;
            }
            case SpecialPokeEntity:
            {
                writer.Write((byte)MessageType.SpecialPoke);
                MessagePackSerializer.Serialize(ref writer, (SpecialPokeEntity)value, options);
                break;
            }
            case TextEntity:
            {
                writer.Write((byte)MessageType.Text);
                MessagePackSerializer.Serialize(ref writer, (TextEntity)value, options);
                break;
            }
            case VideoEntity:
            {
                writer.Write((byte)MessageType.Video);
                MessagePackSerializer.Serialize(ref writer, (VideoEntity)value, options);
                break;
            }
            case XmlEntity:
            {
                writer.Write((byte)MessageType.Xml);
                MessagePackSerializer.Serialize(ref writer, (XmlEntity)value, options);
                break;
            }
            default: { break; }
        }
    }
}

/// <summary>
/// DO NOT MODIFY THE ENUMERATION VALUE!
/// IT WILL CAUSE THE DATABASE TO BECOME INVALID!
/// </summary>
file enum MessageType
{
    BounceFace = 0,
    Face = 1,
    File = 2,
    Forward = 3,
    GreyTip = 4,
    GroupReaction = 5,
    Image = 6,
    Json = 7,
    Keyboard = 8,
    LightApp = 9,
    LongMsg = 10,
    Markdown = 11,
    Marketface = 12,
    Mention = 13,
    MultiMsg = 14,
    Poke = 15,
    Record = 16,
    SpecialPoke = 17,
    Text = 18,
    Video = 19,
    Xml = 20,
}