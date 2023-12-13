namespace Lagrange.Core.Common.Entity;

public class BotStranger
{
    internal BotStranger(uint uin, string uid, string nickname)
    {
        Uin = uin;
        Uid = uid;
        Nickname = nickname;
    }
    
    public uint Uin { get; }
    
    internal string Uid { get; }
    
    public string Nickname { get; }

    public string Avatar => $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
}