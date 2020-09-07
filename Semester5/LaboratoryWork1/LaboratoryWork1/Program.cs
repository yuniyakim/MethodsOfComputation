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
        /// <param name="left">Left border of starting interval</param>
        /// <param name="right">Right border of starting interval</param>
        /// <param name="partition">Interval's partition</param>
        /// <returns>List with roots' intervals</returns>
        private static List<(double, double)> SeparateRoots(double left, double right, int partition)
        {
            if (partition <= 0 || left > right)
            {
                throw new ArgumentOutOfRangeException();
            }

            var h = (right - left) / partition;
            var intervals = new List<(double, double)>();
            var x1 = left;
            var x2 = x1 + h;
            var y1 = Function(x1);
            double y2 = 0;
            while (x2 <= right)
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

            Console.WriteLine($"\nAmount of sign reversal segments: {intervals.Count}.");
            foreach (var interval in intervals)
            {
                Console.WriteLine($"[{interval.Item1}; {interval.Item2}]");
            }
            Console.WriteLine();

            return intervals;
        }

        /// <summary>
        /// Finds the root of the equation in the given interval
        /// </summary>
        /// <param name="left">Left border of the given interval</param>
        /// <param name="right">Right border of the given interval</param>
        /// <param name="epsilon">Given accuracy</param>
        /// <returns>Root in the given interval</returns>
        public static double BisectionFindRoot(double left, double right, double epsilon)
        {
            if (epsilon <= 0 || left > right)
            {
                throw new ArgumentOutOfRangeException();
            }

            double middle = 0;
            var localRight = right;
            var localLeft = left;
            var amountOfSteps = 0;
            while (localRight - localLeft > 2 * epsilon)
            {
                middle = (localRight + localLeft) / 2;
                if (Function(localLeft) * Function(middle) <= 0)
                {
                    localRight = middle;
                }
                else
                {
                    localLeft = middle;
                }
                amountOfSteps++;
            }

            var x = (localLeft + localRight) / 2;
            Console.WriteLine($"Root of the equation: {x}.");
            Console.WriteLine($"Initial approximation: {left}.");
            Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            Console.WriteLine($"Delta: {(localRight - localLeft) / 2}.");
            Console.WriteLine($"Absolute value of the residual: {Function(x)}.");
            Console.WriteLine();

            return x;
        }

        public static void Main()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program finds all the roots of the transcendent equation in the given interval.\n");
            Console.WriteLine($"Function: {function}.");
            Console.WriteLine($"Interval: [{left}, {right}].");
            Console.WriteLine($"Epsilon: {epsilon}.");

            const int partition = 1000;

            var intervals = SeparateRoots(left, right, partition);

            Console.WriteLine("BISECTION METHOD.");
            var roots = new List<double>();
            foreach (var interval in intervals)
            {
                roots.Add(BisectionFindRoot(interval.Item1, interval.Item2, epsilon));
            }
        }
    }
}
