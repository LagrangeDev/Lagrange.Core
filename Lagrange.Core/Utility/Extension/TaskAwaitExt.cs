using System.Runtime.CompilerServices;

namespace Lagrange.Core.Utility.Extension;

internal static class TaskAwaiters
{
    /// <summary>
    /// Returns an awaitable/awaiter that will ensure the continuation is executed
    /// asynchronously on the thread pool, even if the task is already completed
    /// by the time the await occurs.  Effectively, it is equivalent to awaiting
    /// with ConfigureAwait(false) and then queuing the continuation with Task.Run,
    /// but it avoids the extra hop if the continuation already executed asynchronously.
    /// </summary>
    public static ForceAsyncAwaiter ForceAsync(this Task task) => new(task);
}

internal readonly struct ForceAsyncAwaiter : ICriticalNotifyCompletion
{
    private readonly Task _task;

    internal ForceAsyncAwaiter(Task task) => _task = task;

    public ForceAsyncAwaiter GetAwaiter() => this;

    public bool IsCompleted => false; // the purpose of this type is to always force a continuation

    public void GetResult() => _task.GetAwaiter().GetResult();

    public void OnCompleted(Action action) =>
        _task.ConfigureAwait(false).GetAwaiter().OnCompleted(action);

    public void UnsafeOnCompleted(Action action) => 
        _task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(action);
}