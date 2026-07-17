using Xunit;
using System.Threading;
using task17;
using task18;

namespace task18tests;

public class QuantumServerThreadTests
{
    private class QuickCommand : ICommand
    {
        public bool Executed { get; private set; }
        public void Execute() => Executed = true;
    }

    [Fact]
    public void Should_ProcessLongOperations_InParallelWithQuickCommands_WithoutBlocking()
    {
        var scheduler = new QueueScheduler();
        var serverThread = new QuantumServerThread(scheduler);

        var longCmd = new LongCommand(scheduler, 3);
        var quickCmd = new QuickCommand();

        serverThread.Execute(longCmd);
        serverThread.Execute(quickCmd);

        serverThread.Start();
        
        Thread.Sleep(100);
        serverThread.Stop();
        serverThread.Join();

        Assert.Equal(3, longCmd.ExecutionCount);
        Assert.True(quickCmd.Executed);
    }
}
