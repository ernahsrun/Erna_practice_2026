using Xunit;
using System.Collections.Generic;
using System.Threading;
using task17;
using task18;
using task19;

namespace task19tests;

public class LongOperationsTests
{
    [Fact]
    public void Test_FiveCommands_ThreeExecutions_Then_HardStop()
    {
        var scheduler = new QueueScheduler();
        var serverThread = new QuantumServerThread(scheduler);
        var commands = new List<TestCommand>();
        
        int cancelledCount = 0;
        var syncEvent = new ManualResetEvent(false);

        for (int i = 1; i <= 5; i++)
        {
            var cmd = new TestCommand(i, scheduler, maxExecutions: 5);
            
            cmd.OnStepExecuted = (executedCmd) =>
            {
                if (executedCmd.ExecutionCount == 3)
                {
                    executedCmd.Cancel();
                    
                    if (Interlocked.Increment(ref cancelledCount) == 5)
                    {
                        serverThread.Stop();
                        syncEvent.Set();
                    }
                }
            };

            commands.Add(cmd);
            scheduler.Add(cmd);
        }

        serverThread.Start();
        
        syncEvent.WaitOne(2000);
        serverThread.Join();

        foreach (var cmd in commands)
        {
            Assert.Equal(3, cmd.ExecutionCount);
            Assert.True(cmd.IsCancelled);
        }
    }
}
