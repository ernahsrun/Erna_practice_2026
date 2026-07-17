using Xunit;
using System;
using System.Threading;
using task17;

namespace task17tests;

public class ServerThreadTests
{
    private class TestCommand : ICommand
    {
        public int ExecutionCount { get; private set; }
        public void Execute() => ExecutionCount++;
    }

    [Fact]
    public void HardStopCommand_ShouldStopThreadImmediately_LeavingRemainingCommandsUnexecuted()
    {
        var serverThread = new ServerThread();
        var cmd1 = new TestCommand();
        var cmd2 = new TestCommand();
        var hardStop = new HardStopCommand(serverThread);
        var cmd3 = new TestCommand();

        serverThread.Execute(cmd1);
        serverThread.Execute(cmd2);
        serverThread.Execute(hardStop);
        serverThread.Execute(cmd3);

        serverThread.Start();
        serverThread.Join();

        Assert.Equal(1, cmd1.ExecutionCount);
        Assert.Equal(1, cmd2.ExecutionCount);
        Assert.Equal(0, cmd3.ExecutionCount);
    }

    [Fact]
    public void SoftStopCommand_ShouldExecuteAllExistingCommands_BeforeStopping()
    {
        var serverThread = new ServerThread();
        var cmd1 = new TestCommand();
        var cmd2 = new TestCommand();
        var softStop = new SoftStopCommand(serverThread);
        var cmd3 = new TestCommand();

        serverThread.Execute(cmd1);
        serverThread.Execute(cmd2);
        serverThread.Execute(softStop);

        serverThread.Start();
        Thread.Sleep(50); 
        serverThread.Execute(cmd3);
        serverThread.Join();

        Assert.Equal(1, cmd1.ExecutionCount);
        Assert.Equal(1, cmd2.ExecutionCount);
        Assert.Equal(0, cmd3.ExecutionCount);
    }

    [Fact]
    public void External_HardStopCall_ShouldThrowException()
    {
        var serverThread = new ServerThread();
        Assert.Throws<InvalidOperationException>(() => serverThread.HandleHardStop());
    }

    [Fact]
    public void External_SoftStopCall_ShouldThrowException()
    {
        var serverThread = new ServerThread();
        Assert.Throws<InvalidOperationException>(() => serverThread.HandleSoftStop());
    }
}
