using Lagrange.Core.Message.Entity;

namespace Lagrange.Core.Message;

/// <summary>
/// MessageBuilder is used to build the <see cref="MessageChain"/>, every MessageChain should be created with this class
/// </summary>
public sealed class MessageBuilder
{
    private MessageChain _chain;

    private MessageBuilder(MessageChain chain) => _chain = chain;

    public static MessageBuilder Friend(uint friendUin)
    {
        // TODO: Get the real UID
        return new MessageBuilder(new MessageChain(friendUin, "", ""));
    }

    public static MessageBuilder Group(uint groupUin, uint memberUin) => new(new MessageChain(groupUin, memberUin));

    public MessageBuilder Text(string text)
    {
        var textEntity = new TextEntity(text);
        _chain.Add(textEntity);
        
        return this;
    }
    
    public MessageChain Build() => _chain;
}