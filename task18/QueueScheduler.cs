using System.Collections.Generic;
using task17;

namespace task18;

public class QueueScheduler : IScheduler
{
    private readonly Queue<ICommand> _longTasks = new();

    public bool HasCommand() => _longTasks.Count > 0;

    public ICommand Select() => _longTasks.Dequeue();

    public void Add(ICommand cmd) => _longTasks.Enqueue(cmd);
}
