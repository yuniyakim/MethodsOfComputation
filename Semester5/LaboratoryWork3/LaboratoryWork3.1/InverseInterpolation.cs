using System;
using System.Collections.Generic;
using System.Linq;

namespace LaboratoryWork3_1
{
    /// <summary>
    /// Inverse interpolation
    /// </summary>
    public class InverseInterpolation
    {
        private double left = 0; // a
        private double right = 1; // b
        private double point = 0.65;
        private int amountOfNodes = 10; // m
        private int powerOfPolynomial = 7; // n
        private double epsilon = Math.Pow(10, -8);

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
        /// <param name="inverseFlag">Indicates if inverse interpolation is needed</param>
        /// <returns></returns>
        public Dictionary<double, double> NodesTable(double left, double right, int amountOfNodes, bool inverseFlag)
        {
            var table = new Dictionary<double, double>();
            for (var i = 0; i <= amountOfNodes; i++)
            {
                var node = left + i * (right - left) / amountOfNodes;
                if (inverseFlag)
                {
                    table.Add(Function(node), node);
                }
                else
                {
                    table.Add(node, Function(node));
                }
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
        /// <param name="currentPoint">Given point</param>
        /// <returns>Value of Newton polynomial</returns>
        public double NewtonPolynomialValue(List<KeyValuePair<double, double>> table, List<List<double>> dividedDifferences, double currentPoint)
        {
            double value = 0;
            for (var i = 0; i <= powerOfPolynomial; i++)
            {
                double multiplier = 1;
                for (var j = 0; j < i; j++)
                {
                    multiplier *= currentPoint - table[j].Key;
                }
                value += dividedDifferences[0][i] * multiplier;
            }
            return value;
        }

        /// <summary>
        /// Calculates Newton polynomial's value in the given point
        /// </summary>
        /// <param name="currentPoint">Given point</param>
        /// <returns>Newton polynomial's value</returns>
        public double NewtonPolynomialValueInPoint(double currentPoint)
        {
            var table = NodesTable(left, right, amountOfNodes, false);
            var sortedTable = SortedNodesTable(table, point, powerOfPolynomial);
            var dividedDifferences = DividedDifferences(sortedTable);
            return NewtonPolynomialValue(sortedTable, dividedDifferences, currentPoint);
        }

        /// <summary>
        /// Separates roots and adds corresponding intervals into list
        /// </summary>
        /// <param name="left">Left border of starting interval</param>
        /// <param name="right">Right border of starting interval</param>
        /// <param name="partition">Interval's partition</param>
        /// <returns>List with roots' intervals</returns>
        public List<(double, double)> SeparateRoots(double left, double right)
        {
            if (left > right)
            {
                throw new ArgumentOutOfRangeException();
            }

            var h = (right - left) / 500;
            var intervals = new List<(double, double)>();
            var x1 = left;
            var x2 = x1 + h;
            var y1 = NewtonPolynomialValueInPoint(x1) - point;
            double y2 = 0;
            while (x2 <= right)
            {
                y2 = NewtonPolynomialValueInPoint(x2) - point;
                if (y1 * y2 <= 0)
                {
                    intervals.Add((x1, x2));
                }
                x1 = x2;
                x2 = x1 + h;
                y1 = y2;
            }

            //Console.WriteLine($"\nAmount of sign reversal segments: {intervals.Count}.");
            //foreach (var interval in intervals)
            //{
            //    Console.WriteLine($"[{interval.Item1}; {interval.Item2}]");
            //}
            //Console.WriteLine();

            return intervals;
        }

        /// <summary>
        /// Finds the root of the equation in the given interval using bisection method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <returns>Root in the given interval</returns>
        public double BisectionFindRoot(double leftBorder, double rightBorder)
        {
            if (leftBorder > rightBorder)
            {
                throw new ArgumentOutOfRangeException();
            }

            double middle = 0;
            var localRight = rightBorder;
            var localLeft = leftBorder;
            var amountOfSteps = 0;
            while (localRight - localLeft > 2 * epsilon)
            {
                middle = (localRight + localLeft) / 2;
                if ((NewtonPolynomialValueInPoint(localLeft) - point) * (NewtonPolynomialValueInPoint(middle) - point) <= 0)
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
            //Console.WriteLine($"Initial approximation: {leftBorder}.");
            //Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            //Console.WriteLine($"Delta: {(localRight - localLeft) / 2}.");
            Console.WriteLine($"Absolute value of the residual: {Math.Abs(Function(x) - point)}.");
            Console.WriteLine();

            return x;
        }

        /// <summary>
        /// Starts the main process
        /// </summary>
        /// <param name="input">Shows if input is needed</param>
        public void Process(bool input)
        {
            if (input)
            {
                Console.WriteLine("\nPlease, enter value.");
                double F = 0;
                var flagF = double.TryParse(Console.ReadLine(), out F);
                while (!flagF)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double) point.");
                    flagF = double.TryParse(Console.ReadLine(), out F);
                }
                point = F;

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

                Console.WriteLine("\nPlease, enter desired accuracy.");
                double eps = 0;
                var flagEps = double.TryParse(Console.ReadLine(), out eps) && eps > 0;
                while (!flagEps)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double, greater than 0) desired accuracy.");
                    flagEps = double.TryParse(Console.ReadLine(), out eps) && eps > 0;
                }
                epsilon = eps;
            }

            var table = NodesTable(left, right, amountOfNodes, true);

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
            var value = NewtonPolynomialValue(sortedTable, dividedDifferences, point);
            Console.WriteLine($"\nResult of inverse interpolation X = Qn (F = {point}): {value}.");
            Console.WriteLine($"Absolute actual residual |f(X) - F|: {Math.Abs(Function(value) - point)}.");
            Console.WriteLine();

            var intervals = SeparateRoots(left, right);
            if (intervals.Count == 0)
            {
                Console.WriteLine("There are no roots of polynomial.");
            }
            else
            {
                foreach (var interval in intervals)
                {
                    BisectionFindRoot(interval.Item1, interval.Item2);
                }
            }
        }

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program solves the problem of inverse algebraic interpolation.");
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
            }

            Process(false);
        }
    }
}