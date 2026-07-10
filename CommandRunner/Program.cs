using System;
using System.IO;
using System.Reflection;
using CommandLib;

namespace CommandRunner;

class Program
{
    static void Main(string[] args)
    {
        string dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FileSystemCommands.dll");

        if (!File.Exists(dllPath))
        {
            dllPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "FileSystemCommands", "bin", "Debug", "net8.0", "FileSystemCommands.dll");
        }

        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Ошибка: Файл {dllPath} не найден. Сначала соберите решение.");
            return;
        }

        Assembly assembly = Assembly.LoadFrom(dllPath);

        Type? sizeCommandType = assembly.GetType("FileSystemCommands.DirectorySizeCommand");
        Type? findCommandType = assembly.GetType("FileSystemCommands.FindFilesCommand");

        if (sizeCommandType == null || findCommandType == null)
        {
            Console.WriteLine("Ошибка: Не удалось найти классы команд в загруженной библиотеке.");
            return;
        }

        string testPath = Directory.GetCurrentDirectory();

        object? sizeCommandInstance = Activator.CreateInstance(sizeCommandType, new object[] { testPath });
        
        object? findCommandInstance = Activator.CreateInstance(findCommandType, new object[] { testPath, "*.json" });

        if (sizeCommandInstance is ICommand sizeCmd)
        {
            Console.WriteLine("--- Запуск DirectorySizeCommand ---");
            sizeCmd.Execute();
        }

        if (findCommandInstance is ICommand findCmd)
        {
            Console.WriteLine("\n--- Запуск FindFilesCommand ---");
            findCmd.Execute();
        }
    }
}
