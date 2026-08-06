namespace Lagrange.Core.Common.Response;

[Serializable]
public class BotGroupClockInResult
{
    public BotGroupClockInResult() { }

    public BotGroupClockInResult(bool isSuccess)
    {
        IsSuccess = isSuccess;
    }

    public bool IsSuccess { get; set; }

    public string Title { get; set; } = string.Empty;

    public string KeepDayText { get; set; } = string.Empty;

    public string GroupRankText { get; set; } = string.Empty;

    public long ClockInTime { get; set; } = 0;

    public string DetailUrl { get; set; } = string.Empty;

    public static BotGroupClockInResult Fail() => new(false);

    public static BotGroupClockInResult Success() => new(true);
}
