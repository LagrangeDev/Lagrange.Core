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
    
    public MessageBuilder Text(string text)
    {
        var textEntity = new TextEntity(text);
        _chain.Add(textEntity);
        
        return this;
    }
    
    public MessageBuilder Mention(uint target, string? display = null)
    {
        var mentionEntity = new MentionEntity(display ?? target.ToString(), target);
        _chain.Add(mentionEntity);
        
        return this;
    }
    
    public MessageBuilder Face(ushort id, bool isLarge = false)
    {
        var faceEntity = new FaceEntity(id, isLarge);
        _chain.Add(faceEntity);
        
        return this;
    }

    public MessageBuilder Forward(MessageChain target)
    {
        var forwardEntity = new ForwardEntity(target);
        _chain.Add(forwardEntity);
        
        return this;
    }
    
    public MessageBuilder MultiMsg(uint? groupUin = null, params MessageBuilder[] msg)
    {
        var multiMsgEntity = new MultiMsgEntity(groupUin, msg.Select(x => x.Build()).ToList());
        _chain.Add(multiMsgEntity);
        
        return this;
    }
    
    public MessageChain Build() => _chain;
}