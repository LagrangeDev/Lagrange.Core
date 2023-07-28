using System.Text;
using Lagrange.Core.Core.Packets.Message.Element;

namespace Lagrange.Core.Message;

public sealed class MessageChain : List<IMessageEntity>
{
    public uint? GroupUin { get; }
    
    public uint FriendUin { get; }

    internal uint Sequence { get; }
    
    internal string? SelfUid { get; }
    
    internal string? Uid { get; }

    internal bool IsGroup { get; }
    
    internal List<Elem> Elements { get; }

    internal MessageChain(uint friendUin, string selfUid, string friendUid)
    {
        GroupUin = null;
        FriendUin = friendUin;
        Sequence = 0; // unuseful at there
        SelfUid = selfUid;
        Uid = friendUid;
        IsGroup = false;
        Elements = new List<Elem>();
    }

    internal MessageChain(uint groupUin)
    {
        GroupUin = groupUin;
        Sequence = 0; // unuseful at there
        Uid = null;
        IsGroup = true;
        Elements = new List<Elem>();
    }
    
    internal MessageChain(uint groupUin, uint friendUin)
    {
        GroupUin = groupUin;
        FriendUin = friendUin;
        Sequence = 0; // unuseful at there
        Uid = null;
        IsGroup = true;
        Elements = new List<Elem>();
    }
    
    public bool HasTypeOf<T>() where T : IMessageEntity => this.Any(entity => entity is T);
    
    public T? GetEntity<T>() where T : class, IMessageEntity => this.FirstOrDefault(entity => entity is T, null) as T;

    public string ToPreviewString()
    {
        var chainBuilder = new StringBuilder();

        chainBuilder.Append("[MessageChain");
        if (GroupUin != null) chainBuilder.Append($"({GroupUin})");
        chainBuilder.Append($"({FriendUin})");
        chainBuilder.Append("] ");
        foreach (var entity in this)
        {
            chainBuilder.Append(entity.ToPreviewString());
            if (this.Last() != entity) chainBuilder.Append(" | ");
        }
        
        return chainBuilder.ToString();
    }
}