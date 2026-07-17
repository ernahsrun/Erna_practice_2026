using System;
using task17;

namespace task18;

public class LongCommand : ICommand
{
    private readonly IScheduler _scheduler;
    private readonly int _totalSteps;
    private int _currentStep = 0;
    
    public int ExecutionCount => _currentStep;

    public LongCommand(IScheduler scheduler, int totalSteps)
    {
        _scheduler = scheduler;
        _totalSteps = totalSteps;
    }

    public void Execute()
    {
        _currentStep++;
        
        if (_currentStep < _totalSteps)
        {
            _scheduler.Add(this);
        }
    }
}
