using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class FindFilesCommand : ICommand
{
    private readonly string _path;
    private readonly string _searchPattern;

    public FindFilesCommand(string path, string searchPattern)
    {
        _path = path;
        _searchPattern = searchPattern;
    }

    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Каталог '{_path}' не существует.");
            return;
        }

        var files = Directory.GetFiles(_path, _searchPattern, SearchOption.AllDirectories);
        Console.WriteLine($"Найденные файлы по маске '{_searchPattern}' в '{_path}':");
        foreach (var file in files)
        {
            Console.WriteLine(Path.GetFileName(file));
        }
    }
}
