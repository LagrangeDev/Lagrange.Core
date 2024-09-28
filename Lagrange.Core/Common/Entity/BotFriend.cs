namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotFriend
{
    /// <summary>
    /// The empty constructor for serialization
    /// </summary>
    internal BotFriend()
    {
        Uid = string.Empty;
        Nickname = string.Empty;
        Remarks = string.Empty;
        PersonalSign = string.Empty;
        Qid = string.Empty;
        Group = default;
        Avatar = string.Empty;
    }

    internal BotFriend(uint uin, string uid, string nickname, string remarks, string personalSign, string qid, BotFriendGroup group = default)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
        Remarks = remarks;
        PersonalSign = personalSign;
        Qid = qid;
        Group = group;
        Avatar = $"https://q1.qlogo.cn/g?b=qq&nk={uin}&s=640";
    }

    public uint Uin { get; set; }

    internal string Uid { get; set; }

    public string Nickname { get; set; }

    public string Remarks { get; set; }

    public string PersonalSign { get; set; }

    public string Qid { get; set; }

    public BotFriendGroup Group { get; set; }

    public string Avatar { get; set; }
}