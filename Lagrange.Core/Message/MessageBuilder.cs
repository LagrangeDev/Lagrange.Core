using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message;

/// <summary>
/// MessageBuilder is used to build the <see cref="MessageChain"/>, every MessageChain should be created with this class
/// </summary>
public sealed class MessageBuilder
{
    private readonly MessageChain _chain;

    private MessageBuilder(MessageChain chain) => _chain = chain;

    public static MessageBuilder Friend(uint friendUin) => new(new MessageChain(friendUin, "", "")); // automatically set selfUid and friendUid by MessagingLogic

    public static MessageBuilder Group(uint groupUin) => new(new MessageChain(groupUin));
    
    public static MessageBuilder FakeGroup(uint groupUin, uint memberUin) => 
            new(new MessageChain(groupUin, memberUin, (uint)Random.Shared.Next(1000000, 9999999)));
    
    /// <summary>
    /// Add a text entity to the message chain
    /// </summary>
    /// <param name="text">The text to be added</param>
    public MessageBuilder Text(string text)
    {
        var textEntity = new TextEntity(text);
        _chain.Add(textEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a mention entity to the message chain (@someone)
    /// </summary>
    /// <param name="target">the Uin of target</param>
    /// <param name="display">the string to be displayed</param>
    public MessageBuilder Mention(uint target, string? display = null)
    {
        var mentionEntity = new MentionEntity(display, target);
        _chain.Add(mentionEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a face entity to the message chain
    /// </summary>
    /// <param name="id">The id of emoji</param>
    /// <param name="isLarge">Is the emoji large</param>
    public MessageBuilder Face(ushort id, bool isLarge = false)
    {
        var faceEntity = new FaceEntity(id, isLarge);
        _chain.Add(faceEntity);
        
        return this;
    }

    /// <summary>
    /// Add a forward entity to the message chain (reply)
    /// </summary>
    /// <param name="target">The message chain to be forwarded</param>
    public MessageBuilder Forward(MessageChain target)
    {
        var forwardEntity = new ForwardEntity(target);
        _chain.Add(forwardEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a multimsg entity to the message chain (multi message)
    /// </summary>
    /// <param name="groupUin">The group to be sent, if null, the message will be sent as private message</param>
    /// <param name="msg">The messages to be sent</param>
    public MessageBuilder MultiMsg(uint? groupUin = null, params MessageBuilder[] msg)
    {
        var multiMsgEntity = new MultiMsgEntity(groupUin, msg.Select(x => x.Build()).ToList());
        _chain.Add(multiMsgEntity);
        
        return this;
    }
    
    public MessageBuilder MultiMsg(uint? groupUin = null, params MessageChain[] chains)
    {
        var multiMsgEntity = new MultiMsgEntity(groupUin, chains.ToList());
        _chain.Add(multiMsgEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a xml entity to the message chain (card message)
    /// </summary>
    /// <param name="xml">The xml to be sent</param>
    public MessageBuilder Xml(string xml)
    {
        var xmlEntity = new XmlEntity(xml);
        _chain.Add(xmlEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a image entity to the message chain
    /// </summary>
    /// <remarks>The file would not be closed until the message is sent</remarks>
    /// <param name="filePath">The path of image file</param>
    public MessageBuilder Image(string filePath)
    {
        var imageEntity = new ImageEntity(filePath);
        _chain.Add(imageEntity);
        
        return this;
    }
    
    /// <summary>
    /// Add a image entity to the message chain
    /// </summary>
    /// <param name="file">The image file</param>
    public MessageBuilder Image(byte[] file)
    {
        var imageEntity = new ImageEntity(file);
        _chain.Add(imageEntity);
        
        return this;
    }
    
    public MessageBuilder Video(byte[] file, int videoLength = 0)
    {
        var videoEntity = new VideoEntity(file, videoLength);
        _chain.Add(videoEntity);

        return this;
    }

    public MessageBuilder Video(string filePath, int videoLength = 0)
    {
        var videoEntity = new VideoEntity(filePath, videoLength);
        _chain.Add(videoEntity);

        return this;
    }

    /// <summary>
    /// Add a audio entity to the message chain
    /// </summary>
    /// <param name="file">The audio file that has already been converted to SilkCodec</param>
    /// <param name="audioLength">The length of the audio file that directly shown</param>
    public MessageBuilder Record(byte[] file, int audioLength = 0)
    {
        var recordEntity = new RecordEntity(file, audioLength);
        _chain.Add(recordEntity);

        return this;
    }

    /// <summary>
    /// Add a audio entity to the message chain
    /// </summary>
    /// <param name="filePath">The audio file that has already been converted to SilkCodec</param>
    /// <param name="audioLength">The length of the audio file that directly shown</param>
    public MessageBuilder Record(string filePath, int audioLength = 0)
    {
        var recordEntity = new RecordEntity(filePath, audioLength);
        _chain.Add(recordEntity);

        return this;
    }

    /// <summary>
    /// Add a dedicated poke entity to message chain
    /// </summary>
    /// <param name="type">Poke ID, default value is the preset of NTQQ</param>
    public MessageBuilder Poke(uint type = 1)
    {
        var pokeEntity = new PokeEntity(type);
        _chain.Add(pokeEntity);

        return this;
    }
    
    /// <summary>
    /// Add a dedicated LightApp entity to message chain
    /// </summary>
    /// <param name="payload">Json Payload</param>
    public MessageBuilder LightApp(string payload)
    {
        var pokeEntity = new LightAppEntity(payload);
        _chain.Add(pokeEntity);

        return this;
    }
    
    public MessageBuilder LongMsg(string resId)
    {
        var longMsgEntity = new LongMsgEntity(resId);
        _chain.Add(longMsgEntity);
        
        return this;
    }
    
    public MessageBuilder LongMsg(MessageChain chain)
    {
        var longMsgEntity = new LongMsgEntity(chain);
        _chain.Add(longMsgEntity);
        
        return this;
    }
    
    public MessageBuilder Markdown(string json)
    {
        var markdownEntity = new MarkdownEntity(json);
        _chain.Add(markdownEntity);
        
        return this;
    }
    
    public MessageBuilder Markdown(MarkdownData data)
    {
        var markdownEntity = new MarkdownEntity(data);
        _chain.Add(markdownEntity);
        
        return this;
    }
    
    public MessageBuilder Keyboard(string json)
    {
        var keyboardEntity = new KeyboardEntity(json);
        _chain.Add(keyboardEntity);
        
        return this;
    }
    
    public MessageBuilder Keyboard(KeyboardData data)
    {
        var keyboardEntity = new KeyboardEntity(data);
        _chain.Add(keyboardEntity);
        
        return this;
    }
    
    public MessageBuilder MarketFace(string faceId, int tabId, string key, string summary)
    {
        var marketFaceEntity = new MarketfaceEntity(faceId, tabId, key, summary);
        _chain.Add(marketFaceEntity);
        
        return this;
    }
    
    public MessageBuilder Add(IMessageEntity entity)
    {
        _chain.Add(entity);
        return this;
    }
    
    public MessageChain Build() => _chain;
}