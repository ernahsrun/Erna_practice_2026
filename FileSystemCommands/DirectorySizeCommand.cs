using System;
using System.IO;
using CommandLib;

namespace FileSystemCommands;

public class DirectorySizeCommand : ICommand
{
    private readonly string _path;

    public DirectorySizeCommand(string path)
    {
        _path = path;
    }

    public void Execute()
    {
        if (!Directory.Exists(_path))
        {
            Console.WriteLine($"Каталог '{_path}' не существует.");
            return;
        }

        long size = 0;
        var files = Directory.GetFiles(_path, "*", SearchOption.AllDirectories);
        foreach (var file in files)
        {
            size += new FileInfo(file).Length;
        }

        Console.WriteLine($"Размер каталога '{_path}': {size} байт.");
    }
}
