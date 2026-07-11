using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace task10;

public class PluginLoader
{
    public List<string> LoadAndExecutePlugins(string directoryPath)
    {
        var executedPlugins = new List<string>();
        
        if (!Directory.Exists(directoryPath))
            return executedPlugins;

        string[] dllFiles = Directory.GetFiles(directoryPath, "*.dll");
        var pluginTypes = new Dictionary<string, Type>();
        var dependencies = new Dictionary<string, string[]>();

        foreach (var dll in dllFiles)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(dll);
                var types = assembly.GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<PluginLoadAttribute>();
                    if (attr != null)
                    {
                        string pluginName = type.Name;
                        pluginTypes[pluginName] = type;
                        dependencies[pluginName] = attr.Dependencies;
                    }
                }
            }
            catch
            {
            }
        }

        var sortedPlugins = SortPlugins(pluginTypes.Keys.ToList(), dependencies);

        foreach (var name in sortedPlugins)
        {
            if (pluginTypes.TryGetValue(name, out var type))
            {
                var instance = Activator.CreateInstance(type) as ICommand;
                if (instance != null)
                {
                    instance.Execute();
                    executedPlugins.Add(name); 
                }
            }
        }

        return executedPlugins;
    }

    private List<string> SortPlugins(List<string> plugins, Dictionary<string, string[]> dependencies)
    {
        var sorted = new List<string>();
        var visited = new Dictionary<string, bool>();

        foreach (var plugin in plugins)
        {
            if (!visited.ContainsKey(plugin))
            {
                Visit(plugin, dependencies, visited, sorted);
            }
        }

        return sorted;
    }

    private void Visit(string plugin, Dictionary<string, string[]> dependencies, Dictionary<string, bool> visited, List<string> sorted)
    {
        if (visited.TryGetValue(plugin, out bool isVisited))
        {
            if (!isVisited) throw new InvalidOperationException("Обнаружена циклическая зависимость плагинов!");
            return;
        }

        visited[plugin] = false; 

        if (dependencies.TryGetValue(plugin, out var deps))
        {
            foreach (var dep in deps)
            {
                if (dependencies.ContainsKey(dep))
                {
                    Visit(dep, dependencies, visited, sorted);
                }
            }
        }

        visited[plugin] = true; 
        sorted.Add(plugin);     
    }
}
