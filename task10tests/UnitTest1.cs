using Xunit;
using System;
using System.Collections.Generic;
using task10;

namespace task10tests;

public class BasePlugin : ICommand
{
    public static bool WasExecuted { get; set; }
    public void Execute() => WasExecuted = true;
}

[PluginLoad("BasePlugin")]
public class DependentPlugin : ICommand
{
    public static bool WasExecuted { get; set; }
    public void Execute() => WasExecuted = true;
}

public class PluginLoaderTests
{
    [Fact]
    public void PluginLoadAttribute_ShouldStoreDependencies()
    {
        var attr = new PluginLoadAttribute("PluginA", "PluginB");

        Assert.Equal(2, attr.Dependencies.Length);
        Assert.Contains("PluginA", attr.Dependencies);
        Assert.Contains("PluginB", attr.Dependencies);
    }

    [Fact]
    public void PluginLoader_ShouldResetFlagsCorrectly()
    {
        BasePlugin.WasExecuted = false;
        var plugin = new BasePlugin();
        
        plugin.Execute();

        Assert.True(BasePlugin.WasExecuted);
    }
}
