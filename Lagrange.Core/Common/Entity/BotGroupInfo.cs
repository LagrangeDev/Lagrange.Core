namespace Lagrange.Core.Common.Entity;

public class BotGroupInfo
{
    public string OwnerUid { get; set; } = string.Empty;

    public ulong CreateTime { get; set; }

    public ulong MaxMemberCount { get; set; }

    public ulong MemberCount { get; set; }

    public ulong Level { get; set; }

    public string Name { get; set; } = string.Empty;

    public string NoticePreview { get; set; } = string.Empty;

    public ulong Uin { get; set; }

    public ulong LastSequence { get; set; }

    public ulong LastMessageTime { get; set; }

    public string Question { get; set; } = string.Empty;

    public string Answer { get; set; } = string.Empty;

    public ulong MaxAdminCount { get; set; }
}