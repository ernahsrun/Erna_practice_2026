using Xunit;
using System;
using System.IO;
using FileSystemCommands;

namespace task08tests;

public class FileSystemCommandsTests
{
    [Fact]
    public void DirectorySizeCommand_ShouldCalculateSize()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDirSize");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "test1.txt"), "Hello");
        File.WriteAllText(Path.Combine(testDir, "test2.txt"), "World");

        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var command = new DirectorySizeCommand(testDir);
        command.Execute();

        var output = stringWriter.ToString();
        Assert.Contains("10 байт", output);

        Directory.Delete(testDir, true);
    }

    [Fact]
    public void FindFilesCommand_ShouldFindMatchingFiles()
    {
        var testDir = Path.Combine(Path.GetTempPath(), "TestDirFind");
        Directory.CreateDirectory(testDir);
        File.WriteAllText(Path.Combine(testDir, "file1.txt"), "Text");
        File.WriteAllText(Path.Combine(testDir, "file2.log"), "Log");

        var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        var command = new FindFilesCommand(testDir, "*.txt");
        command.Execute();

        var output = stringWriter.ToString();
        Assert.Contains("file1.txt", output);
        Assert.DoesNotContain("file2.log", output);

        Directory.Delete(testDir, true);
    }
}
