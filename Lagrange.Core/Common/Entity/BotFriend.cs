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
    }
    
    internal BotFriend(uint uin,string uid, string nickname, string remarks, string personalSign)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
        Remarks = remarks;
        PersonalSign = personalSign;
    }
    
    public uint Uin { get; }
    
    internal string Uid { get; }
    
    public string Nickname { get; }
    
    public string Remarks { get; }
    
    public string PersonalSign { get; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}