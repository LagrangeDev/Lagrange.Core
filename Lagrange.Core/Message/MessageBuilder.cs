using Lagrange.Core.Message.Entities;

namespace Lagrange.Core.Message;

public class MessageBuilder
{
    private readonly List<IMessageEntity> _entities = [];

    public MessageChain Build() => [.._entities];

    public MessageBuilder Text(string text)
    {
        _entities.Add(new TextEntity(text));
        return this;
    }

    public MessageBuilder Mention(long uin, string? display)
    {
        _entities.Add(new MentionEntity(uin, display));
        return this;
    }

    public MessageBuilder Reply(BotMessage source)
    {
        _entities.Add(new ReplyEntity(source));
        return this;
    }

    public MessageBuilder MultiMsg(List<BotMessage> messages)
    {
        _entities.Add(new MultiMsgEntity(messages));
        return this;
    }
    
    public MessageBuilder MultiMsg(string resId)
    {
        _entities.Add(new MultiMsgEntity(resId));
        return this;
    }

    public MessageBuilder Image(string path, string? summary = "[图片]", int subType = 0)
    {
        var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Image(fs, summary, subType, true);
    }

    public MessageBuilder Image(byte[] image, string? summary = "[图片]", int subType = 0)
    {
        var ms = new MemoryStream(image);
        return Image(ms, summary, subType, true);
    }

    public MessageBuilder Image(Stream stream, string? summary = "[图片]", int subType = 0, bool disposeOnCompletion = false)
    {
        _entities.Add(new ImageEntity(stream, summary, subType, disposeOnCompletion));
        return this;
    }

    public MessageBuilder Record(string path)
    {
        var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Record(fs, true);
    }

    public MessageBuilder Record(byte[] record)
    {
        var ms = new MemoryStream(record);
        return Record(ms, true);
    }

    public MessageBuilder Record(Stream stream, bool disposeOnCompletion = false)
    {
        _entities.Add(new RecordEntity(stream, disposeOnCompletion));
        return this;
    }

    public MessageBuilder Video(string path, string? thumbnail)
    { 
        var videoFs = new FileStream(path, FileMode.Open, FileAccess.Read);
        var thumbnailFs = thumbnail != null ? new FileStream(thumbnail, FileMode.Open, FileAccess.Read) : null;
        return Video(videoFs, thumbnailFs, true);
    }

    public MessageBuilder Video(byte[] video, byte[]? thumbnail)
    {
        var videoStream = new MemoryStream(video);
        var thumbnailStream = thumbnail != null ? new MemoryStream(thumbnail) : null;
        return Video(videoStream, thumbnailStream, true);
    }

    public MessageBuilder Video(Stream video, Stream? thumbnail = null, bool disposeOnCompletion = false)
    {
        _entities.Add(new VideoEntity(video, thumbnail, disposeOnCompletion));
        return this;
    }

    public MessageBuilder LightApp(string json)
    {
        _entities.Add(new LightAppEntity(json));
        return this;
    }

    public static MessageBuilder operator +(MessageBuilder builder, IMessageEntity entity)
    {
        builder._entities.Add(entity);
        return builder;
    }
    
    public static MessageBuilder operator +(MessageBuilder self, MessageBuilder other)
    {
        self._entities.AddRange(other._entities);
        return self;
    }
}