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
        GroupId = 0;
        GroupName = string.Empty;
    }
    
    internal BotFriend(uint uin,string uid, string nickname, string remarks, string personalSign, uint groupId, string groupName)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
        Remarks = remarks;
        PersonalSign = personalSign;
        GroupId = groupId;
        GroupName = groupName;
    }
    
    public uint Uin { get; set; }
    
    internal string Uid { get; set; }
    
    public string Nickname { get; set; }
    
    public string Remarks { get; set; }
    
    public string PersonalSign { get; set; }

    public uint GroupId { get; }
    
    public string GroupName { get; set; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}