using System;
using System.Threading;
using System.Threading.Tasks;

namespace Lagrange.Milky.Utilities;

public class TaskCompletionTracker
{
    private int _count = 0;
    private bool _waiting = false;

    private readonly TaskCompletionSource _tcs = new();

    private readonly Lock _lock = new();

    public void Track(Task task)
    {
        lock (_lock)
        {
            if (_waiting) throw new InvalidOperationException("Tracker is draining, cannot track new tasks.");
            _count++;
        }

        _ = task.ContinueWith(t =>
        {
            lock (_lock)
            {
                _count--;
                if (_waiting && _count == 0) _tcs.TrySetResult();
            }
        }, TaskContinuationOptions.ExecuteSynchronously);
    }

    public Task WaitAllAsync(CancellationToken ct)
    {
        lock (_lock)
        {
            _waiting = true;
            if (_count == 0) _tcs.TrySetResult();
        }

        return _tcs.Task.WaitAsync(ct);
    }
}