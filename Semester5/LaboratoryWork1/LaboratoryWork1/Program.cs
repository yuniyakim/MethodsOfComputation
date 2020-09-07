﻿using System;
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
        /// Finds the root of the equation in the given interval using bisection method
        /// </summary>
        /// <param name="leftBorder">Left border of the given interval</param>
        /// <param name="rightBorder">Right border of the given interval</param>
        /// <returns>Root in the given interval</returns>
        private static double BisectionFindRoot(double leftBorder, double rightBorder)
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
        private static double NewtonFindRoot(double leftBorder, double rightBorder, int multiplicity)
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
        private static double ModifiedNewtonFindRoot(double leftBorder, double rightBorder)
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
        private static double SecantFindRoot(double leftBorder, double rightBorder)
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
            Console.WriteLine("This program finds all the roots of the transcendent equation in the given interval.\n");
            Console.WriteLine($"Function: {function}.");
            Console.WriteLine($"Interval: [{left}, {right}].");
            Console.WriteLine($"Epsilon: {epsilon}.");

            const int partition = 1000;

            var intervals = SeparateRoots(left, right, partition);

            if (epsilon <= 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            Console.WriteLine("\nBISECTION METHOD.");
            var bisectionRoots = new List<double>();
            foreach (var interval in intervals)
            {
                //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
                bisectionRoots.Add(BisectionFindRoot(interval.Item1, interval.Item2));
            }

            Console.WriteLine("\nNEWTON'S METHOD.");
            var newtonRoots = new List<double>();
            foreach (var interval in intervals)
            {
                //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
                newtonRoots.Add(NewtonFindRoot(interval.Item1, interval.Item2, 1));
            }

            Console.WriteLine("\nMODIFIED NEWTON'S METHOD.");
            var modifiedNewtonRoots = new List<double>();
            foreach (var interval in intervals)
            {
                //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
                modifiedNewtonRoots.Add(ModifiedNewtonFindRoot(interval.Item1, interval.Item2));
            }

            Console.WriteLine("\nSECANT'S METHOD.");
            var secantRoots = new List<double>();
            foreach (var interval in intervals)
            {
                //Console.WriteLine($"Current interval: [{interval.Item1}; {interval.Item2}].");
                secantRoots.Add(SecantFindRoot(interval.Item1, interval.Item2));
            }
        }
    }
}
