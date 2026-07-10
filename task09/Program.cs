using System;
using System.IO;
using System.Reflection;

namespace task09;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Ошибка: Передайте путь к файлу библиотеки классов (.dll) в параметрах командной строки.");
            return;
        }

        string dllPath = args[0];
        if (!File.Exists(dllPath))
        {
            Console.WriteLine($"Ошибка: Файл '{dllPath}' не найден.");
            return;
        }

        try
        {
            string info = AnalyzeAssembly(dllPath);
            Console.WriteLine(info);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при анализе библиотеки: {ex.Message}");
        }
    }

    public static string AnalyzeAssembly(string path)
    {
        var sb = new System.Text.StringBuilder();
        Assembly assembly = Assembly.LoadFrom(path);
        
        sb.AppendLine($"--- Анализ метаданных сборки: {assembly.GetName().Name} ---");

        Type[] types = assembly.GetTypes();
        foreach (Type type in types)
        {
            if (!type.IsClass) continue;

            sb.AppendLine($"\nКласс: {type.FullName}");

            var classAttrs = type.GetCustomAttributes();
            foreach (var attr in classAttrs)
            {
                sb.AppendLine($"  [Атрибут класса: {attr.GetType().Name}]");
            }

            var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (var ctor in constructors)
            {
                sb.Append($"  Конструктор: {type.Name}(");
                var ctorParams = ctor.GetParameters();
                sb.Append(string.Join(", ", System.Array.ConvertAll(ctorParams, p => $"{p.ParameterType.Name} {p.Name}")));
                sb.AppendLine(")");
            }

            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly);
            foreach (var method in methods)
            {
                if (method.IsSpecialName) continue;

                sb.AppendLine($"  Метод: {method.ReturnType.Name} {method.Name}");

                var methodAttrs = method.GetCustomAttributes();
                foreach (var attr in methodAttrs)
                {
                    sb.AppendLine($"    [Атрибут метода: {attr.GetType().Name}]");
                }

                var methodParams = method.GetParameters();
                if (methodParams.Length > 0)
                {
                    sb.AppendLine("    Параметры:");
                    foreach (var p in methodParams)
                    {
                        sb.AppendLine($"      {p.ParameterType.Name} {p.Name}");
                    }
                }
            }
        }

        return sb.ToString();
    }
}
