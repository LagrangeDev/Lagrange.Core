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
        Group = string.Empty;
    }
    
    internal BotFriend(uint uin,string uid, string nickname, string remarks, string personalSign, string group)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
        Remarks = remarks;
        PersonalSign = personalSign;
        Group = group;
    }
    
    public uint Uin { get; set; }
    
    internal string Uid { get; set; }
    
    public string Nickname { get; set; }
    
    public string Remarks { get; set; }
    
    public string PersonalSign { get; set; }

    public string Group { get; set; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}