namespace Lagrange.Core.Common.Entity;

[Serializable]
public class BotGroupClockInResult
{
    public BotGroupClockInResult() { }

    public BotGroupClockInResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    /// <summary>
    /// Is the clock in successful
    /// </summary>
    public bool IsSuccess { get; set; } = false;

    /// <summary>
    /// Maybe "今日已成功打卡"
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Maybe "已打卡N天"
    /// </summary>
    public string KeepDayText { get; set; } = string.Empty;

    /// <summary>
    /// Maybe "群内排名第N位"
    /// </summary>
    public string GroupRankText { get; set; } = string.Empty;

    /// <summary>
    /// The utc time of clock in
    /// </summary>
    public DateTime ClockInUtcTime { get; set; } = DateTime.UnixEpoch; // 打卡时间

    /// <summary>
    /// Detail info url
    /// </summary>
    public string DetailUrl { get; set; } = string.Empty;  // https://qun.qq.com/v2/signin/detail?...

    public static BotGroupClockInResult Fail() => new BotGroupClockInResult()
    {
        IsSuccess = false
    };

    public static BotGroupClockInResult Success() => new BotGroupClockInResult()
    {
        IsSuccess = true
    };
}
