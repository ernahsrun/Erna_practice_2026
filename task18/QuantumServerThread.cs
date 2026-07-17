using System;
using System.Collections.Concurrent;
using System.Threading;
using task17;

namespace task18;

public class QuantumServerThread
{
    private readonly ConcurrentQueue<ICommand> _incomingQueue = new();
    private readonly IScheduler _scheduler;
    private readonly Thread _thread;
    private readonly AutoResetEvent _waitHandle = new(false);
    private bool _isStopped = false;

    public QuantumServerThread(IScheduler scheduler)
    {
        _scheduler = scheduler;
        _thread = new Thread(RunLoop);
    }

    public void Start() => _thread.Start();
    public void Join() => _thread.Join();
    public void Stop() { _isStopped = true; _waitHandle.Set(); }

    public void Execute(ICommand command)
    {
        _incomingQueue.Enqueue(command);
        _waitHandle.Set();
    }

    private void RunLoop()
    {
        while (!_isStopped || !_incomingQueue.IsEmpty || _scheduler.HasCommand())
        {
            bool processedAny = false;

            if (_incomingQueue.TryDequeue(out var incomingCmd))
            {
                incomingCmd.Execute();
                processedAny = true;
            }

            if (_scheduler.HasCommand())
            {
                var longCmd = _scheduler.Select();
                longCmd.Execute();
                processedAny = true;
            }

            if (!processedAny && !_isStopped)
            {
                _waitHandle.WaitOne();
            }
        }
    }
}
