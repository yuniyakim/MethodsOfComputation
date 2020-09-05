using System;
using System.Collections.Generic;

namespace LaboratoryWork1
{
    public class Program
    {
        private static readonly string function = "f(x) = x * sin(x) - 1";

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
        private List<(double, double)> RootsSeparation(double A, double B, int N)
        {
            var h = (B - A) / N;
            var list = new List<(double, double)>();
            var x1 = A;
            var x2 = x1 + h;
            var y1 = Function(x1);
            double y2 = 0;
            while (x2 <= B)
            {
                y2 = Function(x2);
                if (y1*y2 <= 0)
                {
                    list.Add((x1, x2));
                }
                x1 = x2;
                x2 = x1 + h;
                y1 = y2;
            }
            Console.WriteLine(list.Count);
            foreach (var interval in list)
            {
                Console.WriteLine($"[{interval.Item1}; {interval.Item2}]");
            }
            return list;
        }

        public static void Main()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program finds all the roots of the equation in the given interval.\n");
            Console.WriteLine($"Function: {function}.\n");

            Console.WriteLine("Please, enter the left border of desired interval.");
            Console.WriteLine("NOTE: doubles are being entered with dot, NOT comma.");
            double A = 0;
            var flagA = double.TryParse(Console.ReadLine(), out A);
            while (!flagA)
            {
                Console.WriteLine("Please, enter the CORRECT (double) left border of desired interval.");
                flagA = double.TryParse(Console.ReadLine(), out A);
            }

            Console.WriteLine("\nPlease, enter the right border of desired interval.");
            double B = 0;
            var flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
            while (!flagB)
            {
                Console.WriteLine($"Please, enter the CORRECT (double, greater than A = {A}) right border of desired interval.");
                flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
            }

            Console.WriteLine($"\nInterval: [{A}, {B}].");
        }
    }
}
