using System;
using Xunit;
using task14;

namespace task14tests;

public class DefiniteIntegralTests
{
    [Fact]
    public void Test_Integral_Calculations()
    {
        Func<double, double> X = (double x) => x;
        Func<double, double> SIN = (double x) => Math.Sin(x);

        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, X, 1e-4, 2), 3);
        Assert.Equal(0, DefiniteIntegral.Solve(-1, 1, SIN, 1e-5, 8), 3);
        Assert.Equal(10, DefiniteIntegral.Solve(0, 5, X, 1e-6, 8), 3);
    }
}
