using static Lagrange.Core.Common.Entity.BotUserInfo;

namespace Lagrange.OneBot.Utility;

public static class GenderExt
{
    public static string ToOneBotString(this GenderInfo info)
    {
        return info switch
        {
            GenderInfo.Male => "male",
            GenderInfo.Female => "female",
            GenderInfo.Unset or _ => "unknown"
        };
    }
}
