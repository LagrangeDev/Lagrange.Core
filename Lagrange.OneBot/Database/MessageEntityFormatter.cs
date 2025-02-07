using Lagrange.Core.Message;
using Lagrange.Core.Message.Entity;
using MessagePack;
using MessagePack.Formatters;

namespace Lagrange.OneBot.Database;

public class MessageEntityFormatter : IMessagePackFormatter<IMessageEntity?>
{
    /// <summary>
    /// DO NOT MODIFY THE ENUMERATION VALUE!
    /// IT WILL CAUSE THE DATABASE TO BECOME INVALID!
    /// 
    /// Each newly added IMessageEntity should be given a sequence number
    /// </summary>
    private static readonly Dictionary<byte, Type> ID_TYPE = new() {
        { 0, typeof(BounceFaceEntity) },
        { 1, typeof(FaceEntity) },
        { 2, typeof(FileEntity) },
        { 3, typeof(ForwardEntity) },
        { 4, typeof(GreyTipEntity) },
        { 5, typeof(GroupReactionEntity) },
        { 6, typeof(ImageEntity) },
        { 7, typeof(JsonEntity) },
        { 8, typeof(KeyboardEntity) },
        { 9, typeof(LightAppEntity) },
        { 10, typeof(LongMsgEntity) },
        { 11, typeof(MarkdownEntity) },
        { 12, typeof(MarketfaceEntity) },
        { 13, typeof(MentionEntity) },
        { 14, typeof(MultiMsgEntity) },
        { 15, typeof(PokeEntity) },
        { 16, typeof(RecordEntity) },
        { 17, typeof(SpecialPokeEntity) },
        { 18, typeof(TextEntity) },
        { 19, typeof(VideoEntity) },
        { 20, typeof(XmlEntity) },
    };

    private static readonly Dictionary<Type, byte> TYPE_ID = ID_TYPE.ToDictionary(m => m.Value, m => m.Key);

    public IMessageEntity? Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
    {
        byte id = reader.ReadByte();

        return (IMessageEntity?)MessagePackSerializer.Deserialize(ID_TYPE[id], ref reader, options);
    }

    public void Serialize(ref MessagePackWriter writer, IMessageEntity? value, MessagePackSerializerOptions options)
    {
        if (value == null) return;

        var type = value.GetType();

        writer.Write(TYPE_ID[type]);
        MessagePackSerializer.Serialize(type, ref writer, value, options);
    }
}