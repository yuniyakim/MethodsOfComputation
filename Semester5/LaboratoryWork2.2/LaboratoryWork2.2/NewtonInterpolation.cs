using System;
using System.Collections.Generic;
using System.Linq;

namespace LaboratoryWork2_2
{
    /// <summary>
    /// Newton interpolation
    /// </summary>
    public class NewtonInterpolation
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
        /// Created table with nodes and function values
        /// </summary>
        /// <param name="left">Left border of interval</param>
        /// <param name="right">Right border of interval</param>
        /// <param name="amountOfNodes">Amount of nodes</param>
        /// <returns></returns>
        public Dictionary<double, double> NodesTable(double left, double right, int amountOfNodes)
        {
            var table = new Dictionary<double, double>();
            for (var i = 0; i <= amountOfNodes; i++)
            {
                var node = left + i * (right - left) / amountOfNodes;
                table.Add(node, Function(node));
            }
            return table;
        }

        /// <summary>
        /// Sorts and trims table with nodes according to the distance to the point in ascending order
        /// </summary>
        /// <param name="table">Nodes table</param>
        /// <param name="point">Point</param>
        /// <param name="powerOfPolynomial">Power of polynomial</param>
        /// <returns>Table with sorted nodes</returns>
        public List<KeyValuePair<double, double>> SortedNodesTable(Dictionary<double, double> table, double point, int powerOfPolynomial)
        {
            var sortedTable = table.OrderBy(x => Math.Abs(point - x.Key)).ToList();
            sortedTable.RemoveRange(powerOfPolynomial + 1, sortedTable.Count - powerOfPolynomial - 1);
            return sortedTable;
        }

        /// <summary>
        /// Calculates divided differences
        /// </summary>
        /// <param name="table">Table with nodes and function values in nodes</param>
        /// <returns>Divided differences</returns>
        public List<List<double>> DividedDifferences(List<KeyValuePair<double, double>> table)
        {
            var dividedDifferences = new List<List<double>>(powerOfPolynomial + 1);
            for (var i = 0; i <= powerOfPolynomial; i++)
            {
                dividedDifferences.Add(new List<double>());
                for (var j = 0; j <= powerOfPolynomial; j++)
                {
                    dividedDifferences[i].Add(0);
                }
            }

            for (var j = 0; j <= powerOfPolynomial; j++)
            {
                for (var i = 0; i <= powerOfPolynomial; i++)
                {
                    if (j == 0)
                    {
                        dividedDifferences[i][i] = table[i].Value;
                    }
                    else if (i + j <= powerOfPolynomial)
                    {
                        dividedDifferences[i][i + j] = (dividedDifferences[i + 1][i + j] - dividedDifferences[i][i + j - 1]) / 
                            (table[i + j].Key - table[i].Key);
                    }
                }
            }
            return dividedDifferences;
        }

        /// <summary>
        /// Calculates value of Newton polynomial
        /// </summary>
        /// <param name="table">Table with nodes and function values in nodes</param>
        /// <returns>Value of Newton polynomial</returns>
        public double NewtonPolynomialValue(List<KeyValuePair<double, double>> table, List<List<double>> dividedDifferences)
        {
            double value = 0;
            for (var i = 0; i <= powerOfPolynomial; i++)
            {
                double multiplier = 1;
                for (var j = 0; j < i; j++)
                {
                    multiplier *= point - table[j].Key;
                }
                value += dividedDifferences[0][i] * multiplier;
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

            var table = NodesTable(left, right, amountOfNodes);

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

            var sortedTable = SortedNodesTable(table, point, powerOfPolynomial);
            Console.WriteLine($"\nNodes used for calculating interpolation polynomial sorted by distance to the point = {point} in ascending order:");
            foreach (var element in sortedTable)
            {
                Console.WriteLine(element.Key);
            }

            var dividedDifferences = DividedDifferences(sortedTable);
            var value = NewtonPolynomialValue(sortedTable, dividedDifferences);
            Console.WriteLine($"\nValue of Newton polynomial in point = {point}: {value}.");

            Console.WriteLine($"Absolute actual error = |f(x) - Pn(x)|: {Math.Abs(Function(point) - value)}.");
        }

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program solves the problem of algebraic interpolation with Newton method.");
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