using System;
using System.Collections.Generic;
using System.Globalization;

namespace LaboratoryWork1
{
    public class Program
    {
        private static readonly string function = "f(x) = x * sin(x) - 1";
        private static double epsilon = Math.Pow(10, -5);
        private static double left = -10;
        private static double right = 2;
        private static int partition = 1000;

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Function's value</returns>
        private static double Function(double x) => x * Math.Sin(x) - 1;

        /// <summary>
        /// Function's first derivative
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>First derivative's value</returns>
        private static double FirstDerivative(double x) => Math.Sin(x) + x * Math.Cos(x);

        /// <summary>
        /// Function's second derivative
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Second derivative's value</returns>
        private static double SecondDerivative(double x) => 2 * Math.Cos(x) - x * Math.Sin(x);

        /// <summary>
        /// Separates roots and adds corresponding intervals into list
        /// </summary>
        /// <param name="left">Left border of starting interval</param>
        /// <param name="right">Right border of starting interval</param>
        /// <param name="partition">Interval's partition</param>
        /// <returns>List with roots' intervals</returns>
        public static List<(double, double)> SeparateRoots(double left, double right, int partition)
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
        /// Finds the root of the equation in the given interval using bisection method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <returns>Root in the given interval</returns>
        public static double BisectionFindRoot(double leftBorder, double rightBorder)
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
            Console.WriteLine($"Initial approximation: {leftBorder}.");
            Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            Console.WriteLine($"Delta: {(localRight - localLeft) / 2}.");
            Console.WriteLine($"Absolute value of the residual: {Math.Abs(Function(x))}.");
            Console.WriteLine();

            return x;
        }

        /// <summary>
        /// Finds the root of the equation in the given interval using Newton's method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <param name="multiplicity">Current multiplicity</param>
        /// <returns>Root in the given interval</returns>
        public static double NewtonFindRoot(double leftBorder, double rightBorder, int multiplicity)
        {
            if (leftBorder > rightBorder)
            {
                throw new ArgumentOutOfRangeException();
            }

            var current = Function(leftBorder) * SecondDerivative(leftBorder) > 0 ? leftBorder : rightBorder;
            var initialApproximation = current;

            if (FirstDerivative(current) == 0)
            {
                return NewtonFindRoot(leftBorder, rightBorder, multiplicity + 2);
            }

            var next = current - multiplicity * Function(current) / FirstDerivative(current);
            var amountOfSteps = 1;
            while (Math.Abs(next - current) > epsilon)
            {
                current = next;

                if (FirstDerivative(current) == 0)
                {
                    return NewtonFindRoot(leftBorder, rightBorder, multiplicity + 2);
                }

                next = current - multiplicity * Function(current) / FirstDerivative(current);
                amountOfSteps++;
            }

            var x = next;
            Console.WriteLine($"Root of the equation: {x}.");
            Console.WriteLine($"Initial approximation: {initialApproximation}.");
            Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            Console.WriteLine($"Delta: {Math.Abs(next - current)}.");
            Console.WriteLine($"Absolute value of the residual: {Math.Abs(Function(x))}.");
            Console.WriteLine($"Multiplicity: {multiplicity}.");
            Console.WriteLine();

            return next;
        }

        /// <summary>
        /// Finds the root of the equation in the given interval using modified Newton's method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <returns>Root in the given interval</returns>
        public static double ModifiedNewtonFindRoot(double leftBorder, double rightBorder)
        {
            if (leftBorder > rightBorder)
            {
                throw new ArgumentOutOfRangeException();
            }

            var current = Function(leftBorder) * SecondDerivative(leftBorder) > 0 ? leftBorder : rightBorder;
            var initialApproximation = current;
            var initialApproximationDerivative = FirstDerivative(initialApproximation);

            var next = current - Function(current) / initialApproximationDerivative;
            var amountOfSteps = 1;
            while (Math.Abs(next - current) > epsilon)
            {
                current = next;
                next = current - Function(current) / initialApproximationDerivative;
                amountOfSteps++;
            }

            var x = next;
            Console.WriteLine($"Root of the equation: {x}.");
            Console.WriteLine($"Initial approximation: {initialApproximation}.");
            Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            Console.WriteLine($"Delta: {Math.Abs(next - current)}.");
            Console.WriteLine($"Absolute value of the residual: {Math.Abs(Function(x))}.");
            Console.WriteLine();

            return next;
        }

        /// <summary>
        /// Finds the root of the equation in the given interval using secant's method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <returns>Root in the given interval</returns>
        public static double SecantFindRoot(double leftBorder, double rightBorder)
        {
            if (leftBorder > rightBorder)
            {
                throw new ArgumentOutOfRangeException();
            }

            var previous = leftBorder;
            var current = rightBorder;
            var next = current - Function(current) * (current - previous) / (Function(current) - Function(previous));
            var amountOfSteps = 1;
            while (Math.Abs(current - previous) > epsilon)
            {
                previous = current;
                current = next;
                next = current - Function(current) * (current - previous) / (Function(current) - Function(previous));
                amountOfSteps++;
            }

            var x = current;
            Console.WriteLine($"Root of the equation: {x}.");
            Console.WriteLine($"Initial approximation: {leftBorder}, {rightBorder}.");
            Console.WriteLine($"Amount of steps to the root: {amountOfSteps}.");
            Console.WriteLine($"Delta: {Math.Abs(current - previous)}.");
            Console.WriteLine($"Absolute value of the residual: {Math.Abs(Function(x))}.");
            Console.WriteLine();

            return next;
        }

        public static void Main()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program finds all the roots of the transcendent equation in the given interval.");
            Console.WriteLine();

            Console.WriteLine("Do you want to enter parameters? Y/N");
            Console.WriteLine("NOTE: if N, then program will use default parameters.");

            var input = Console.ReadLine();
            while (input != "Y" && input != "N")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }

            if (input == "Y")
            {

                Console.WriteLine("Please, enter the left border of desired interval.");
                Console.WriteLine("NOTE: doubles are being entered with dot, NOT comma.");
                double A = 0;
                var flagA = double.TryParse(Console.ReadLine(), out A);
                while (!flagA)
                {
                    Console.WriteLine("Please, enter the CORRECT (double) left border of desired interval.");
                    flagA = double.TryParse(Console.ReadLine(), out A);
                }
                left = A;

                Console.WriteLine("\nPlease, enter the right border of desired interval.");
                double B = 0;
                var flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                while (!flagB)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double, greater than A = {A}) right border of desired interval.");
                    flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                }
                right = B;

                Console.WriteLine("\nPlease, enter desired accuracy.");
                double eps = 0;
                var flagEps = double.TryParse(Console.ReadLine(), out eps) && eps > 0;
                while (!flagEps)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double, greater than 0) desired accuracy.");
                    flagEps = double.TryParse(Console.ReadLine(), out eps) && eps > 0;
                }
                epsilon = eps;

                Console.WriteLine("\nPlease, enter desired partition.");
                var part = 0;
                var flagPart = int.TryParse(Console.ReadLine(), out part) && part > 0;
                while (!flagPart)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) desired partition.");
                    flagPart = int.TryParse(Console.ReadLine(), out part) && part > 0;
                }
                partition = part;
            }

            Console.WriteLine();
            Console.WriteLine($"Function: {function}.");
            Console.WriteLine($"Interval: [{left}, {right}].");
            Console.WriteLine($"Epsilon: {epsilon}.");
            Console.WriteLine($"Partition: {partition}.");

            var intervals = SeparateRoots(left, right, partition);

            if (epsilon <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var bisectionRoots = new List<double>();
            var newtonRoots = new List<double>();
            var modifiedNewtonRoots = new List<double>();
            var secantRoots = new List<double>();

            //Console.WriteLine("\nBISECTION METHOD.");
            //foreach (var interval in intervals)
            //{
            //    //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
            //    bisectionRoots.Add(BisectionFindRoot(interval.Item1, interval.Item2));
            //}

            //Console.WriteLine("\nNEWTON'S METHOD.");
            //foreach (var interval in intervals)
            //{
            //    //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
            //    newtonRoots.Add(NewtonFindRoot(interval.Item1, interval.Item2, 1));
            //}

            //Console.WriteLine("\nMODIFIED NEWTON'S METHOD.");
            //foreach (var interval in intervals)
            //{
            //    //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
            //    modifiedNewtonRoots.Add(ModifiedNewtonFindRoot(interval.Item1, interval.Item2));
            //}

            //Console.WriteLine("\nSECANT'S METHOD.");
            //foreach (var interval in intervals)
            //{
            //    //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
            //    secantRoots.Add(SecantFindRoot(interval.Item1, interval.Item2));
            //}

            var number = 1;
            foreach (var interval in intervals)
            {
                Console.WriteLine();
                Console.WriteLine($"\nINTERVAL NUMBER {number}!");
                Console.WriteLine($"[{interval.Item1}; {interval.Item2}].");

                Console.WriteLine("\nBISECTION METHOD.");
                bisectionRoots.Add(Program.BisectionFindRoot(interval.Item1, interval.Item2));

                Console.WriteLine("\nNEWTON'S METHOD.");
                newtonRoots.Add(Program.NewtonFindRoot(interval.Item1, interval.Item2, 1));

                Console.WriteLine("\nMODIFIED NEWTON'S METHOD.");
                modifiedNewtonRoots.Add(Program.ModifiedNewtonFindRoot(interval.Item1, interval.Item2));

                Console.WriteLine("\nSECANT'S METHOD.");
                secantRoots.Add(Program.SecantFindRoot(interval.Item1, interval.Item2));

                number++;
            }
        }
    }
}
