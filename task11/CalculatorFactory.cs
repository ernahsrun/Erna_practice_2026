using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace task11;

public static class CalculatorFactory
{
    public static object CreateCalculator()
    {
        string sourceCode = @"
        using System;

        public class Calculator : task11tests.ICalculator
        {
            public int Add(int a, int b) => a + b;
            public int Minus(int a, int b) => a - b;
            public int Mul(int a, int b) => a * b;
            public int Div(int a, int b) => b == 0 ? throw new DivideByZeroException() : a / b;
        }";

        var syntaxTree = CSharpSyntaxTree.ParseText(sourceCode);

        string assemblyName = Path.GetRandomFileName();
        var compilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

        var references = new[]
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Console).Assembly.Location),
            MetadataReference.CreateFromFile(Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll")),
            MetadataReference.CreateFromFile(Assembly.Load("task11tests").Location)
        };

        var compilation = CSharpCompilation.Create(assemblyName, new[] { syntaxTree }, references, compilationOptions);

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);

        if (!result.Success)
        {
            var failures = result.Diagnostics.Where(diagnostic => diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error);
            throw new InvalidOperationException($"Ошибка компиляции строки: {string.Join(", ", failures.Select(f => f.GetMessage()))}");
        }

        ms.Seek(0, SeekOrigin.Begin);
        var assembly = Assembly.Load(ms.ToArray());
        var type = assembly.GetType("Calculator") ?? throw new TypeLoadException("Не удалось найти тип Calculator в сборке.");

        return Activator.CreateInstance(type) ?? throw new InvalidOperationException("Не удалось создать объект.");
    }
}
