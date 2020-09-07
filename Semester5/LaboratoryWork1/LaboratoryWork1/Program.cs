using System;
using System.Collections.Generic;

namespace LaboratoryWork1
{
    public class Program
    {
        private static readonly string function = "f(x) = x * sin(x) - 1";
        private static readonly double epsilon = Math.Pow(10, -5);
        private static readonly double left = -10;
        private static readonly double right = 2;

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Parameter x</param>
        /// <returns>Function's value</returns>
        private static double Function(double x) => x * Math.Sin(x) - 1;

        /// <summary>
        /// Separates roots and adds corresponding intervals into list
        /// </summary>
        /// <param name="A">Left border of starting interval</param>
        /// <param name="B">Right border of starting interval</param>
        /// <param name="N">Partition</param>
        /// <returns>List with roots' sintervals</returns>
        private static List<(double, double)> RootsSeparation(double A, double B, int N)
        {
            if (N <= 0 || A > B)
            {
                throw new ArgumentOutOfRangeException();
            }

            var h = (B - A) / N;
            var intervals = new List<(double, double)>();
            var x1 = A;
            var x2 = x1 + h;
            var y1 = Function(x1);
            double y2 = 0;
            while (x2 <= B)
            {
                y2 = Function(x2);
                if (y1*y2 <= 0)
                {
                    intervals.Add((x1, x2));
                }
                x1 = x2;
                x2 = x1 + h;
                y1 = y2;
            }
            Console.WriteLine(intervals.Count);
            foreach (var interval in intervals)
            {
                Console.WriteLine($"[{interval.Item1}; {interval.Item2}]");
            }
            return intervals;
        }

        public static void Main()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program finds all the roots of the transcendent equation in the given interval.\n");
            Console.WriteLine($"Function: {function}.");
            Console.WriteLine($"Interval: [{left}, {right}].");
            Console.WriteLine($"Epsilon: {epsilon}.");

            const int partition = 1000;

            var intervals = RootsSeparation(left, right, partition);
        }
    }
}
