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
        FriendGroupId = 0;
    }

    internal BotFriend(uint uin, string uid, string nickname, string remarks, string personalSign, string qid, uint friendGroupId)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
        Remarks = remarks;
        PersonalSign = personalSign;
        Qid = qid;
        FriendGroupId = friendGroupId;
    }

    public uint Uin { get; set; }

    internal string Uid { get; set; }

    public string Nickname { get; set; }

    public string Remarks { get; set; }

    public string PersonalSign { get; set; }

    public string Qid { get; set; }

    public uint FriendGroupId { get; set; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}