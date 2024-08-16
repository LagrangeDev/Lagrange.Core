using System.Text;
using Lagrange.Core.Common.Entity;
using Lagrange.Core.Internal.Packets.Message.Element;

namespace Lagrange.Core.Message;

public sealed class MessageChain : List<IMessageEntity>
{
    public MessageType Type { get; set; }

    public uint? GroupUin { get; }

    public uint FriendUin { get; }

    public uint TargetUin { get; }

    public ulong MessageId { get; }

    public DateTime Time { get; internal set; }

    public BotFriend? FriendInfo { get; internal set; }

    public BotGroupMember? GroupMemberInfo { get; internal set; }

    public uint Sequence { get; internal set; }

    #region Internal Properties

    internal string? SelfUid { get; set; }

    internal string? Uid { get; set; }

    internal bool IsGroup { get; set; }

    internal List<Elem> Elements { get; set; }

    #endregion

    internal MessageChain(uint friendUin, string selfUid, string friendUid, uint targetUin = 0, uint sequence = 0, ulong? messageId = null,
        MessageType type = MessageType.Friend)
    {
        GroupUin = null;
        FriendUin = friendUin;
        TargetUin = targetUin;
        Sequence = sequence; // unuseful at there
        SelfUid = selfUid;
        Uid = friendUid;
        MessageId = messageId ?? (0x10000000ul << 32) | (uint)Random.Shared.Next(100000000, int.MaxValue);
        IsGroup = false;
        Elements = new List<Elem>();
        Type = type;
    }

    internal MessageChain(uint groupUin)
    {
        GroupUin = groupUin;
        Sequence = 0; // unuseful at there
        MessageId = (0x10000000ul << 32) | (uint)Random.Shared.Next(100000000, int.MaxValue);
        Uid = null;
        IsGroup = true;
        Elements = new List<Elem>();
    }

    internal MessageChain(uint groupUin, uint friendUin, uint sequence, ulong messageId = 0)
    {
        GroupUin = groupUin;
        FriendUin = friendUin;
        Sequence = sequence;
        Uid = null;
        MessageId = messageId;
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

    public string ToPreviewText()
    {
        var chainBuilder = new StringBuilder();

        foreach (var entity in this)
        {
            chainBuilder.Append(entity.ToPreviewText());
        }

        return chainBuilder.ToString();
    }

    public enum MessageType
    {
        Group,
        Temp,
        Friend
    }
}
