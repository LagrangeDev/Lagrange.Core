using Lagrange.Core.Core.Packets.Message.Element;

namespace Lagrange.Core.Message;

public sealed class MessageChain : List<IMessageEntity>
{
    public uint? GroupUin { get; }
    
    public uint FriendUin { get; }

    internal uint Sequence { get; }
    
    internal string? Uid { get; }

    internal bool IsGroup { get; }
    
    internal List<Elem> Elements { get; }

    internal MessageChain(uint friendUin, string friendUid)
    {
        GroupUin = null;
        FriendUin = friendUin;
        Sequence = 0; // TODO: Allocate a dedicated sequence to this field
        Uid = friendUid;
        IsGroup = false;
        Elements = new List<Elem>();
    }

    internal MessageChain(uint groupUin, uint memberUin)
    {
        GroupUin = groupUin;
        FriendUin = memberUin;
        Sequence = 0; // TODO: Allocate a dedicated sequence to this field
        Uid = null;
        IsGroup = true;
        Elements = new List<Elem>();
    }
    
    public bool HasTypeOf<T>() where T : IMessageEntity => this.Any(entity => entity is T);
    
    public T? GetEntity<T>() where T : class, IMessageEntity => this.First(entity => entity is T) as T;
}