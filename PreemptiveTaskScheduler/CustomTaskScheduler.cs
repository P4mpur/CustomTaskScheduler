using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


public class CustomTaskScheduler
{
    public CustomTaskScheduler(int maxDegreeOfParallelism, Mode mode)
    {
        if (maxDegreeOfParallelism < 1) throw new ArgumentOutOfRangeException("Maks degree must be more than 0");
        this.maxDegreeOfParallelism = maxDegreeOfParallelism; ;
        this.executingTasks = new Task[maxDegreeOfParallelism];
        this.mode = mode;
    }

    public enum Mode
    {
        Preemptive,
        NonPreemptive
    }

    private Mode mode;

    private readonly int maxDegreeOfParallelism;

    private readonly object locker = new object();

    public delegate void FunctionForExecution();

    public class MyToken
    {
        public bool isOnWait { get; private set; }

        public void SetWait() => isOnWait = true;

        public void ResetWait() => isOnWait = false;

        public bool IsCanceled { get; private set; }

        public void Cancel() => IsCanceled = true;

        public EventWaitHandle Handler = new EventWaitHandle(false, EventResetMode.AutoReset);

        public Task MyTask { get; set; }


    }


    public MyToken CreateMyToken()
    {
        return new MyToken();
    }


    class TaskData
    {
        public TaskData(MyToken tkn, int pr, int maxduration, Task mytask)
        {
            this.Token = tkn;
            this.maxDuration = maxduration;
            this.priority = pr;
            Working = false;
            taskWithCallback = null;
            Token.MyTask = mytask;
        }



        public int priority { get; set; }

        public int maxDuration { get; set; }

        public Task taskWithCallback { get; set; }

        public bool Working { get; set; }

        public Stopwatch stopwatch = new Stopwatch();

        public MyToken Token { get; set; }

    }

    public int CurrentTaskCount => pendingTasks.Count;

    public int executingTasksCount => executingTasks.Length;

    Dictionary<Task, TaskData> database = new Dictionary<Task, TaskData>();

    SortedList<int, Task> pendingTasks = new SortedList<int, Task>(new CompareForSameKey<int>());

    Task[] executingTasks;

    public void ScheduleTask(FunctionForExecution func, MyToken token, int priority, int maxDuration)
    {
        Task task = new Task(() => func());
        TaskData td = new TaskData(token, priority, maxDuration, task);

        database.Add(task, td);
        pendingTasks.Add(priority, task);

        SchedulePendingTasks();
    }

    private void SchedulePendingTasks()
    {
        lock (locker)
        {
            AbortTasksOvertime();

            if (mode == Mode.Preemptive)
            {
                ScheduleTasksPreemptive();
            }
            else
            {
                ScheduleTasksNonPreemptive();
            }
        }
    }

    private void ScheduleTasksPreemptive()
    {
        int[] availableThreads = executingTasks.Select((value, i) => (value, i)).Where(x => x.value == null).Select(x => x.i).ToArray();
        if (availableThreads.Length > 0)
        {
            foreach (var free in availableThreads)
            {
                Task task = pendingTasks.Values.FirstOrDefault(x => !database[x].Working && !x.IsCompleted);

                if (task != null)
                {
                    database[task].Working = true;
                    if (database[task].Token.isOnWait)
                    {
                        database[task].Token.ResetWait();
                        database[task].Token.Handler.Set();

                        Task taskWithCallback = Task.Factory.StartNew(() =>
                        {
                            long timeLeft = database[task].maxDuration * 1000 - (int)database[task].stopwatch.ElapsedMilliseconds;
                            
                            Task.Delay((int)timeLeft).Wait();
                            if (!database[task].Token.isOnWait)
                            {
                                Console.WriteLine("database[task].IsOnWait: Zadatak : " + task.Id + "se otkazuje");
                                Console.WriteLine("database[task] " + task.Id + " : " + database[task].Working);
                                Console.WriteLine("database[task]  stopwatch : " + database[task].stopwatch + " time left : " + timeLeft);
                                database[task].Token.Cancel();
                                task.Wait();
                                SchedulePendingTasks();
                            }
                        });
                        database[task].taskWithCallback = taskWithCallback;
                        database[task].stopwatch.Restart();
                    }
                    else
                    {
                        task.Start();
                        database[task].stopwatch.Start();
                        Task taskWithCallback = Task.Factory.StartNew(() =>
                        {
                            Task.Delay(database[task].maxDuration * 1000 ).Wait();
                            if (!database[task].Token.isOnWait)
                            {
                                database[task].Token.Cancel();
                                task.Wait();
                                SchedulePendingTasks();
                            }
                        });
                        database[task].taskWithCallback = taskWithCallback;
                    }
                    executingTasks[free] = task;
                }

            }
        }

        var potentialExec = pendingTasks.Where(x => !database[x.Value].Working).ToList();
        var lista = executingTasks.Select((value, i) => (value, i)).Where(x => x.value != null).OrderByDescending(x => database[x.value].priority).ToList();

        foreach (var pair in potentialExec)
        {
            if (lista.Count > 0 && database[pair.Value].priority < database[lista[0].value].priority)
            {
                Task t = lista[0].value;
                int index = lista[0].i;
                database[t].Token.SetWait();
                database[t].stopwatch.Stop();
                database[t].Working = false;

                Task forWork = pair.Value;
                database[forWork].Working = true;

                
                if (database[forWork].Token.isOnWait)
                {
                    database[forWork].Token.ResetWait();
                    database[forWork].Token.Handler.Set();
                    database[forWork].stopwatch.Reset();

                    Task taskWithCallback = Task.Factory.StartNew(() =>
                    {
                        long timeLeft = database[forWork].maxDuration * 1000 - database[forWork].stopwatch.ElapsedMilliseconds;
                        Task.Delay((int)timeLeft).Wait();
                        if (!database[forWork].Token.isOnWait)
                        {
                            database[forWork].Token.Cancel();
                            forWork.Wait();
                            SchedulePendingTasks();
                        }
                    });
                    database[forWork].taskWithCallback = taskWithCallback;
                    database[forWork].stopwatch.Restart();
                }
                else
                {
                    forWork.Start();
                    database[forWork].stopwatch.Start();
                    Task taskWithCallback = Task.Factory.StartNew(() =>
                    {
                        Task.Delay(database[forWork].maxDuration * 1000).Wait();
                        if (!database[forWork].Token.isOnWait)
                        {
                            database[forWork].Token.Cancel();
                            forWork.Wait();
                            SchedulePendingTasks();
                        }
                    });
                    database[forWork].taskWithCallback = taskWithCallback;
                }
                executingTasks[index] = forWork;
                lista.RemoveAt(0);
            }
        }

    }

    private void ScheduleTasksNonPreemptive()
    {
        int[] availableThreads = executingTasks.Select((value, i) => (value, i)).Where(x => x.value == null).Select(x => x.i).ToArray();

        foreach (var free in availableThreads)
        {
            Task task = pendingTasks.Values.FirstOrDefault(x => !database[x].Working && !x.IsCompleted);

            if (task != null)
            {
                database[task].Working = true;
                task.Start();
                Task taskWithCallback = Task.Factory.StartNew(() =>
                {
                    Task.Delay(database[task].maxDuration * 1000).Wait();
                    database[task].Token.Cancel();
                    task.Wait();
                    SchedulePendingTasks();
                });
                database[task].taskWithCallback = taskWithCallback;
                executingTasks[free] = task;
            }

        }
    }

    private void AbortTasksOvertime()
    {
        for (int i = 0; i < maxDegreeOfParallelism; i++)
        {
            if (executingTasks[i] != null)
            {
                Task executingTask = executingTasks[i];
                if (executingTask.IsCanceled || executingTask.IsCompleted)
                {
                    executingTasks[i] = null;
                    database[executingTask].Token.Cancel();
                    pendingTasks.RemoveAt(pendingTasks.IndexOfValue(executingTask));
                }

            }
        }
    }
}

internal class CompareForSameKey<T> : IComparer<T> where T : IComparable
{
    public int Compare(T x, T y)
    {
        int result = x.CompareTo(y);

        if (result == 0)
            return -1;
        else
            return result;
    }
}