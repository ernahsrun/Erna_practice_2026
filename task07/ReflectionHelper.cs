using System;
using System.Reflection;

namespace task07;

public static class ReflectionHelper
{
    public static void PrintTypeInfo(Type type)
    {
        if (type == null) return;

        var classAttr = type.GetCustomAttribute<DisplayNameAttribute>();
        if (classAttr != null)
        {
            Console.WriteLine($"Класс: {classAttr.DisplayName}");
        }

        var versionAttr = type.GetCustomAttribute<VersionAttribute>();
        if (versionAttr != null)
        {
            Console.WriteLine($"Версия: {versionAttr.Major}.{versionAttr.Minor}");
        }
    }
}