using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace LaboratoryWork2
{
    /// <summary>
    /// Lagrange interpolation
    /// </summary>
    public class LagrangeInterpolation
    {
        private double left = 0;
        private double right = 1;
        private double point = 0.65;
        private int amountOfNodes = 15; // m
        private int powerOfPolynomial = 7; // n

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double Function(double x) => 1 - Math.Exp(-2 * x);

        /// <summary>
        /// Calculates value of lagrange polynomial
        /// </summary>
        /// <param name="table">Table with nodes and function values in nodes</param>
        /// <returns>Value of lagrange polynomial</returns>
        private double LagrangePolynomialValue(List<KeyValuePair<double, double>> table)
        {
            double value = 0;
            for (var k = 0; k <= powerOfPolynomial; k++)
            {
                double coefficient = 1;
                for (var i = 0; i <= powerOfPolynomial; i++)
                {
                    if (i != k)
                    {
                        coefficient = coefficient * (point - table[i].Key) / (table[k].Key - table[i].Key);
                    }
                }
                value += coefficient * table[k].Value;
            }
            return value;
        }

        /// <summary>
        /// Starts the main process
        /// </summary>
        /// <param name="input">Shows if input is needed</param>
        public void Process(bool input)
        {
            if (input)
            {
                Console.WriteLine("\nPlease, enter the power of polynomial.");
                Console.WriteLine($"NOTE: the power of polynomial must be less than or equal to the amount of nodes - 1 = {amountOfNodes}.");
                var n = 0;
                var flagN = int.TryParse(Console.ReadLine(), out n) && n > 0 && n <= amountOfNodes;
                while (!flagN)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0, less than or equal to the amount of nodes - 1 = {amountOfNodes}) power of polynomial.");
                    flagN = int.TryParse(Console.ReadLine(), out n) && n > 0 && n <= amountOfNodes;
                }
                powerOfPolynomial = n;

                Console.WriteLine("\nPlease, enter point.");
                double x = 0;
                var flagX = double.TryParse(Console.ReadLine(), out x);
                while (!flagX)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double) point.");
                    flagX = double.TryParse(Console.ReadLine(), out x);
                }
                point = x;
            }

            var table = new Dictionary<double, double>();
            for (var i = 0; i <= amountOfNodes; i++)
            {
                var node = left + i * (right - left) / amountOfNodes;
                table.Add(node, Function(node));
            }

            Console.WriteLine();
            Console.WriteLine($"Interval: [{left}, {right}].");
            Console.WriteLine($"Amount of nodes: {amountOfNodes + 1}.");
            Console.WriteLine($"Table:");
            Console.WriteLine($"xj | f(xj)");
            foreach (var element in table)
            {
                Console.WriteLine($"{element.Key} | {element.Value}");
            }
            Console.WriteLine($"Power of polynomial: {powerOfPolynomial}.");
            Console.WriteLine($"Point: {point}.");

            var orderedTable = table.OrderBy(x => Math.Abs(point - x.Key)).ToList();
            orderedTable.RemoveRange(powerOfPolynomial + 1, orderedTable.Count - powerOfPolynomial - 1);
            Console.WriteLine($"\nNodes used for calculating interpolation polynomial sorted by distance to the point = {point} in ascending order:");
            foreach (var element in orderedTable)
            {
                Console.WriteLine(element.Key);
            }

            var value = LagrangePolynomialValue(orderedTable);
            Console.WriteLine($"\nValue of lagrange polynomial in point = {point}: {value}.");

            Console.WriteLine($"Absolute actual error = |f(x) - Pn(x)|: {Math.Abs(Function(point) - value)}.");
        }

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program solves the problem of algebraic interpolation with Lagrange method.");
            Console.WriteLine("Option 5.");
            Console.WriteLine();

            Console.WriteLine("Do you want to enter parameters? Y/N");
            Console.WriteLine("NOTE: if N, program will use default values of parameters.");

            var input = Console.ReadLine();
            while (input != "Y" && input != "N")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }

            if (input == "Y")
            {
                Console.WriteLine("Please, enter the left border of interval.");
                Console.WriteLine("NOTE: doubles are being entered with dot, NOT comma.");
                double A = 0;
                var flagA = double.TryParse(Console.ReadLine(), out A);
                while (!flagA)
                {
                    Console.WriteLine("Please, enter the CORRECT (double) left border of interval.");
                    flagA = double.TryParse(Console.ReadLine(), out A);
                }
                left = A;

                Console.WriteLine("\nPlease, enter the right border of interval.");
                double B = 0;
                var flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                while (!flagB)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double, greater than A = {A}) right border of interval.");
                    flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                }
                right = B;

                Console.WriteLine("\nPlease, enter amount of nodes.");
                var m = 0;
                var flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                while (!flagM)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of nodes.");
                    flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                }
                amountOfNodes = m - 1;
            }

            Process(input == "Y");
        }
    }
}
