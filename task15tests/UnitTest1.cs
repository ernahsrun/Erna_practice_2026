using Xunit;
using Xunit.Abstractions;
using task15;

namespace task15tests;

public class PerformanceTests
{
    private readonly ITestOutputHelper _output;

    public PerformanceTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public void FullAnalysis_ShouldRunAndComparePerformance()
    {
        string report = IntegralResearch.RunFullAnalysis();

        _output.WriteLine(report);

        Assert.NotEmpty(report);
        Assert.Contains("СРАВНЕНИЕ ПРОИЗВОДИТЕЛЬНОСТИ", report);
        Assert.Contains("Ускорение за счет многопоточности", report);
    }
}
