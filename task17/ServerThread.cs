using System;
using System.Collections.Concurrent;
using System.Threading;

namespace task17;

public class ServerThread
{
    private readonly ConcurrentQueue<ICommand> _queue = new();
    private readonly Thread _thread;
    private readonly AutoResetEvent _waitHandle = new(false);
    
    private Action _currentStrategy;
    private bool _isStopped = false;

    public ServerThread()
    {
        _currentStrategy = DefaultStrategy;
        _thread = new Thread(RunLoop);
    }

    public void Start() => _thread.Start();
    public void Join() => _thread.Join();

    public void Execute(ICommand command)
    {
        if (_isStopped) return;

        _queue.Enqueue(command);
        _waitHandle.Set();
    }

    private void RunLoop()
    {
        while (!_isStopped || !_queue.IsEmpty)
        {
            _currentStrategy();
        }
    }

    private void DefaultStrategy()
    {
        if (_queue.TryDequeue(out var command))
        {
            ProcessCommand(command);
        }
        else
        {
            if (!_isStopped)
            {
                _waitHandle.WaitOne();
            }
        }
    }

    private void SoftStopStrategy()
    {
        if (_queue.TryDequeue(out var command))
        {
            ProcessCommand(command);
        }
        else
        {
            _isStopped = true;
        }
    }

    private void ProcessCommand(ICommand command)
    {
        try
        {
            command.Execute();
        }
        catch (Exception ex)
        {
            ExceptionHandler.Handle(command, ex);
        }
    }

    public void HandleHardStop()
    {
        if (Thread.CurrentThread.ManagedThreadId != _thread.ManagedThreadId)
        {
            throw new InvalidOperationException("Команда HardStop должна выполняться только в целевом потоке.");
        }

        _isStopped = true;
        _queue.Clear();
        _waitHandle.Set();
    }

    public void HandleSoftStop()
    {
        if (Thread.CurrentThread.ManagedThreadId != _thread.ManagedThreadId)
        {
            throw new InvalidOperationException("Команда SoftStop должна выполняться только в целевом потоке.");
        }

        _currentStrategy = SoftStopStrategy;
        _waitHandle.Set();
    }
}

public static class ExceptionHandler
{
    public static void Handle(ICommand command, Exception ex)
    {
    }
}
