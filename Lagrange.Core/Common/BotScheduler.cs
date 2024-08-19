// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

namespace Lagrange.Core.Common;

/// <summary>
/// Task Scheduler
/// </summary>
public class Scheduler
{
    public const int Infinity = int.MaxValue;
    
    private BotContext Bot { get; }
    
    private Utility.TaskScheduler Instance { get; }
    
    public string Name { get; }
    
    public Action<BotContext> Action { get; }

    internal Scheduler(BotContext bot, string name, Action<BotContext> action)
    {
        Bot = bot;
        Name = name;
        Action = action;
        Instance = bot.Scheduler;
    }
    
    ~Scheduler() => Cancel();

    /// <summary>
    /// Create a task scheduler
    /// </summary>
    /// <param name="bot"><b>[In]</b> Bot instance</param>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <param name="action"><b>[In]</b> Task callback action</param>
    /// <returns></returns>
    public static Scheduler Create(BotContext bot, string name, Action<BotContext> action) => new(bot, name, action);

    /// <summary>
    /// Execute the task with a specific interval
    /// </summary>
    /// <param name="interval"><b>[In]</b> Interval in milliseconds</param>
    /// <param name="times"><b>[In]</b> Execute times</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Interval(int interval, int times) => Instance.Interval(Name, interval, times, () => Action(Bot));

    /// <summary>
    /// Execute the task with a specific interval infinity
    /// </summary>
    /// <param name="interval"><b>[In]</b> Interval in milliseconds</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Interval(int interval) => Instance.Interval(Name, interval, Infinity, () => Action(Bot));

    /// <summary>
    /// Execute the task once
    /// </summary>
    /// <param name="delay"><b>[In]</b> Delay time in milliseconds</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void RunOnce(int delay) => Instance.RunOnce(Name, delay, () => Action(Bot));

    /// <summary>
    /// Execute the task once
    /// </summary>
    /// <param name="date"><b>[In]</b> Execute date</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void RunOnce(DateTimeOffset date) => Instance.RunOnce(Name, date, () => Action(Bot));

    /// <summary>
    /// Trigger a task to run
    /// </summary>
    public void Trigger() => Instance.Trigger(Name);

    /// <summary>
    /// Cancel the task
    /// </summary>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Cancel() => Instance.Cancel(Name);
}