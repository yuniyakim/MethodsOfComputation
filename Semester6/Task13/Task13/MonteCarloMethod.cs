using System;
using System.Collections.Generic;

namespace Task13
{
    public static class MonteCarloMethod
    {
        public static double MonteCarlo(int N, double a, double b, string p)
        {
            var random = new Random();
            var X = new List<double>();
            double sum = 0;
            if (p == "uniform")
            {
                for (var i = 0; i < N; i++)
                {
                    X.Add(random.NextDouble() * (b - a) + a);
                    sum += GFunction(X[i]);
                }
                return (b - a) * sum / N;
            }
            else
            {
                while (X.Count < N)
                {
                    var pointX = random.NextDouble();
                    var pointY = random.NextDouble();
                    var x0 = a + pointX * (b - a);
                    var y0 = pointY * 4 / Math.PI;
                    if (0 <= y0 && y0 <= LinearP(x0))
                    {
                        X.Add(x0);
                        sum += GFunction(x0) / LinearP(x0);
                    }
                }
                return sum / N;
            }
        }

        public static double MonteCarloArea(int N, double a, double b)
        {
            var k = 0;
            var random = new Random();
            var X = new List<(double, double)>();
            for (var i = 0; i < N; i++)
            {
                X.Add((random.NextDouble() * (b - a) + a, random.NextDouble()));
            }
            for (var i = 0; i < N; i++)
            {
                if (X[i].Item2 <= GFunction(X[i].Item1))
                {
                    k++;
                }
            }
            return k * Math.PI / 2 / N;
        }

        private static double GFunction(double x) => Math.Cos(x);

        private static double LinearP(double x) => 4 / Math.PI - 8 * x / Math.Pow(Math.PI, 2);
    }
}
