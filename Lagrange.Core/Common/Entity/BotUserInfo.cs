namespace Lagrange.Core.Common.Entity;

public class BotUserInfo
{
    internal BotUserInfo(uint uin, string nickname, DateTime birthday, string city, string country, string school, uint age, DateTime registerTime, uint gender, string? qid, uint level)
    {
        Uin = uin;
        Avatar = $"https://q1.qlogo.cn/g?b=qq&nk={Uin}&s=640";
        Nickname = nickname;
        Birthday = birthday;
        City = city;
        Country = country;
        School = school;
        Age = age;
        RegisterTime = registerTime;
        Gender = (GenderInfo)gender;
        Qid = qid;
        Level = level;
    }

    public uint Uin { get; set; }
    
    public string Avatar { get; set; }
    
    public string Nickname { get; set; }
    
    public DateTime Birthday { get; set; }
    
    public string City { get; set; }
    
    public string Country { get; set; }
    
    public string School { get; set; }
    
    public uint Age { get; set; }
    
    public DateTime RegisterTime { get; set; }
    
    public GenderInfo Gender { get; set; }
    
    public string? Qid { get; set; }
    
    public uint Level { get; set; }

    public enum GenderInfo
    {
        Unset = 0,
        Male = 1,
        Female = 2,
        Unknown = 255
    }
}