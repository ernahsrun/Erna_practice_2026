using System;
using task17;
using task18;

namespace task19;

public class TestCommand : ICommand
{
    private readonly int _id;
    private readonly IScheduler _scheduler;
    private readonly int _maxExecutions;
    private int _counter = 0;
    private bool _isCancelled = false;

    public Action<TestCommand>? OnStepExecuted { get; set; }

    public int ExecutionCount => _counter;
    public bool IsCancelled => _isCancelled;

    public TestCommand(int id, IScheduler scheduler, int maxExecutions = 5)
    {
        _id = id;
        _scheduler = scheduler;
        _maxExecutions = maxExecutions;
    }

    public void Cancel()
    {
        _isCancelled = true;
    }

    public void Execute()
    {
        if (_isCancelled)
        {
            return;
        }

        _counter++;
        Console.WriteLine($"Поток {_id} вызов {_counter}");

        OnStepExecuted?.Invoke(this);

        if (_counter < _maxExecutions && !_isCancelled)
        {
            _scheduler.Add(this);
        }
    }
}