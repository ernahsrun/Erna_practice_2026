using Xunit;
using System;
using task11;

namespace task11tests;

public interface ICalculator
{
    int Add(int a, int b);
    int Minus(int a, int b);
    int Mul(int a, int b);
    int Div(int a, int b);
}

public class CalculatorTests
{
    [Fact]
    public void Calculator_ShouldPerformArithmeticOperations_WithoutReflection()
    {
        ICalculator calc = (ICalculator)CalculatorFactory.CreateCalculator();

        Assert.Equal(15, calc.Add(10, 5));
        Assert.Equal(5, calc.Minus(10, 5));
        Assert.Equal(50, calc.Mul(10, 5));
        Assert.Equal(2, calc.Div(10, 5));
    }

    [Fact]
    public void Calculator_ShouldThrowException_WhenDivideByZero()
    {
        ICalculator calc = (ICalculator)CalculatorFactory.CreateCalculator();

        Assert.Throws<DivideByZeroException>(() => calc.Div(10, 0));
    }
}
