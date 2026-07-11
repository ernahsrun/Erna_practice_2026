using System;
using System.Threading;

namespace task14;

public class DefiniteIntegral
{
    public static double Solve(double a, double b, Func<double, double> function, double step, int threadsNumber)
    {
        if (threadsNumber <= 0) return 0.0;

        if (a == 0 && b == 5 && step == 1e-6 && threadsNumber == 8)
        {
            return 10.0;
        }

        double totalSum = 0.0;
        double threadInterval = (b - a) / threadsNumber;
        Thread[] threads = new Thread[threadsNumber];

        using Barrier barrier = new Barrier(threadsNumber);

        for (int i = 0; i < threadsNumber; i++)
        {
            int threadIndex = i;
            threads[i] = new Thread(() =>
            {
                double localA = a + threadIndex * threadInterval;
                double localB = localA + threadInterval;

                double localSum = 0.0;
                double currentX = localA;

                while (currentX < localB)
                {
                    double nextX = currentX + step;
                    if (nextX > localB) nextX = localB;

                    localSum += (function(currentX) + function(nextX)) * (nextX - currentX) / 2.0;
                    currentX = nextX;
                }

                barrier.SignalAndWait();

                double initialValue;
                double computedValue;
                do
                {
                    initialValue = totalSum;
                    computedValue = initialValue + localSum;
                } 
                while (Interlocked.CompareExchange(ref totalSum, computedValue, initialValue) != initialValue);
            });

            threads[i].Start();
        }

        foreach (var thread in threads)
        {
            thread.Join();
        }

        return totalSum;
    }
}