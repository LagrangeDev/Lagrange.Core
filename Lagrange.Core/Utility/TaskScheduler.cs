using System.Collections.Concurrent;

// ReSharper disable InvertIf
// ReSharper disable FunctionNeverReturns
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable PrivateFieldCanBeConvertedToLocalVariable
// ReSharper disable ArrangeObjectCreationWhenTypeNotEvident

namespace Lagrange.Core.Utility;

internal class TaskScheduler : IDisposable
{
    private class Schedule
    {
        public const int Infinity = int.MaxValue;

        /// <summary>
        /// Task name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Task action
        /// </summary>
        public Action Action { get; }

        /// <summary>
        /// Task interval
        /// </summary>
        public int Interval { get; }

        /// <summary>
        /// Execute how many times
        /// </summary>
        public int Times { get; }

        /// <summary>
        /// Task remain interval
        /// </summary>
        internal int RemainInterval { get; set; }

        /// <summary>
        /// Task remain times
        /// </summary>
        internal int RemainTimes { get; set; }

        public Schedule(string name, Action action, int interval, int times)
        {
            Name = name;
            Action = action;
            Interval = interval;
            Times = times;
        }

        public override int GetHashCode()
            => Name.GetHashCode();
    }

    private class ScheduleSorter : IComparer<Schedule>
    {
        public int Compare(Schedule? x, Schedule? y) => x!.RemainInterval - y!.RemainInterval;
    }

    private readonly Thread _taskThread;
    private readonly ConcurrentDictionary<string, Schedule> _taskDict;
    private readonly ManualResetEvent _taskNotify;
    private bool _taskThreadExit;

    public TaskScheduler()
    {
        _taskDict = new ConcurrentDictionary<string, Schedule>();
        _taskNotify = new ManualResetEvent(false);
        _taskThread = new Thread(SchedulerThread);
        _taskThreadExit = false;

        _taskThread.Start();  // Start task thread
    }

    public void Dispose()
    {
        _taskThreadExit = true;
        _taskNotify.Set();
        _taskThread.Join();
    }

    /// <summary>
    /// Scheduler thread
    /// </summary>
    private void SchedulerThread()
    {
        int minInterval;
        DateTime startTime;
        List<Schedule> todoList;
        List<Schedule> taskTable;
        ScheduleSorter taskSorter;
        {
            todoList = new();
            taskTable = new();
            taskSorter = new();

            while (!_taskThreadExit) // Scheduler steps
            {
                Update();
                WaitOne();
                DoTheTask();
            }
        }

        void Update() // Select the task
        {
            if (_taskDict.Count != taskTable.Count)
            {
                // Try get the new tasks from outside
                foreach (var (_, value) in _taskDict)
                {
                    if (taskTable.Find(i => i.GetHashCode() == value.GetHashCode()) == null)
                    {
                        // Set the value
                        value.RemainTimes = value.Times;
                        value.RemainInterval = value.Interval;

                        // Join the queue
                        taskTable.Add(value);
                        // LogI(TAG, $"Join the task => {key}");
                    }
                }
            }

            // Sort the task
            taskTable.Sort(taskSorter);

            // Pickup minimal interval to wait
            minInterval = 0;
            if (taskTable.Count > 0)
            {
                minInterval = taskTable[0].RemainInterval;
            }
        }

        // Wait the task and
        // calculate the remaining
        void WaitOne()
        {
            startTime = DateTime.Now;
            {
                // Set sleep time
                var sleepTime = minInterval == 0
                    ? int.MaxValue
                    : minInterval;

                // Note: If the sleep time less than zero
                // We need to run the task immediately
                if (sleepTime >= 0)
                {
                    // Cache broken 
                    // Please ref issue #129
                    if (_taskDict.Count != taskTable.Count) return;

                    // Reset event and wait
                    _taskNotify.Reset();
                    _taskNotify.WaitOne(sleepTime);
                }
            }
            var passedTime = (int)((DateTime.Now - startTime).TotalSeconds * 1000);

            todoList.Clear();  // Calculate the remain
            for (var i = taskTable.Count - 1; i >= 0; --i)
            {
                // Reduce the interval
                taskTable[i].RemainInterval -= passedTime;

                // Task timeout
                if (taskTable[i].RemainInterval <= 0)
                {
                    if (taskTable[i].Times != Schedule.Infinity) // Reduce the counter
                    {
                        taskTable[i].RemainTimes -= 1;
                    }
                    else
                    {
                        taskTable[i].RemainInterval = taskTable[i].Interval; // Reset the interval
                    }

                    // Mark the tasks
                    // we are going to do
                    todoList.Add(taskTable[i]);
                }

                if (taskTable[i].RemainTimes <= 0)  // Cleanup died tasks
                {
                    _taskDict.TryRemove(taskTable[i].Name, out _);
                    taskTable.RemoveAt(i);
                }
            }
        }

        void DoTheTask() // Do the task
        {
            foreach (var i in todoList)
            {
                // Perform tasks
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    try
                    {
                        i.Action.Invoke();
                    }
                    catch (Exception)
                    {
                        // LogE(TAG, "Task failed.");
                        // LogE(TAG, e);
                    }
                });
            }
        }
    }

    /// <summary>
    /// Cancel the task
    /// </summary>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Cancel(string name)
    {
        // Remove the task
        if (_taskDict.TryGetValue(name, out var task))
        {
            task.RemainTimes = -1;  // Cancel the task
            task.RemainInterval = Schedule.Infinity;

            _taskNotify.Set(); // Wakeup the scheduler thread
        }
    }

    /// <summary>
    /// Execute the task with a specific interval
    /// </summary>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <param name="interval"><b>[In]</b> Interval in milliseconds</param>
    /// <param name="times"><b>[In]</b> Execute times</param>
    /// <param name="action"><b>[In]</b> Callback action</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Interval(string name, int interval, int times, Action action)
    {
        var task = new Schedule(name, action, interval, times);

        if (_taskDict.ContainsKey(name)) _taskDict[name] = task;  // Check duplicate

        _taskDict.TryAdd(name, task); // Add new task
        _taskNotify.Set(); // Wakeup the scheduler thread
    }

    /// <summary>
    /// Execute the task with a specific interval
    /// </summary>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <param name="interval"><b>[In]</b> Interval in milliseconds</param>
    /// <param name="action"><b>[In]</b> Callback action</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void Interval(string name, int interval, Action action)
        => Interval(name, interval, Schedule.Infinity, action);

    /// <summary>
    /// Execute the task once
    /// </summary>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <param name="delay"><b>[In]</b> Delay time in milliseconds</param>
    /// <param name="action"><b>[In]</b> Callback action</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void RunOnce(string name, int delay, Action action)
        => Interval(name, delay, 1, action);

    /// <summary>
    /// Execute the task once
    /// </summary>
    /// <param name="name"><b>[In]</b> Task identity name</param>
    /// <param name="date"><b>[In]</b> Execute date</param>
    /// <param name="action"><b>[In]</b> Callback action</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ObjectDisposedException"></exception>
    public void RunOnce(string name, DateTime date, Action action)
        => RunOnce(name, (int)((date - DateTime.Now).TotalSeconds * 1000), action);

    /// <summary>
    /// Trigger a task to run
    /// </summary>
    /// <param name="name"></param>
    public void Trigger(string name)
    {
        if (!_taskDict.TryGetValue(name, out var task)) return;  // Check the task

        task.RemainInterval = 0; // Set interval to 0
        _taskNotify.Set(); // Wakeup the scheduler thread
    }
}