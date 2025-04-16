namespace Lagrange.Core.Common.Entity;

public class BotStrangerGroupInfo
{
    public ulong CreateTime { get; set; }

    public ulong MaxMemberCount { get; set; }

    public ulong MemberCount { get; set; }

    public string Name { get; set; } = string.Empty;

    public ulong Uin { get; set; }
}