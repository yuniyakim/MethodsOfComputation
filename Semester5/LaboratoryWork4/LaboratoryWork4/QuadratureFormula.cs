using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace LaboratoryWork4
{
    /// <summary>
    /// Quadrature formulas
    /// </summary>
    public class QuadratureFormula
    {
        private double left; // A
        private double right; // B
        private int amountOfIntervals; // m
        private double delta; // h
        private double sumY;
        private double sumP;

        private readonly string zeroDegreePolynomailFunction = "f(x) = 6";

        /// <summary>
        /// Function of zero degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunction(double x) => 6;

        /// <summary>
        /// Function of zero degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunctionIntegral(double x) => 6 * x;

        private readonly string firstDegreePolynomailFunction = "f(x) = 4 * x + 8";

        /// <summary>
        /// Function of first degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double FirstDegreePolynomailFunction(double x) => 4 * x + 8;

        /// <summary>
        /// Function of first degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double FirstDegreePolynomailFunctionIntegral(double x) => 2 * x * x + 8 * x;

        private readonly string secondDegreePolynomailFunction = "f(x) = 3 * x ^ 2 - 2 * x + 7";

        /// <summary>
        /// Function of second degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SecondDegreePolynomailFunction(double x) => 3 * x * x - 2 * x + 7;

        /// <summary>
        /// Function of second degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SecondDegreePolynomailFunctionIntegral(double x) => x * x * x - x * x + 7 * x;

        private readonly string thirdDegreePolynomailFunction = "f(x) = 8 * x ^ 3 - 3 * x ^ 2 + 4 * x - 10";

        /// <summary>
        /// Function of third degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ThirdDegreePolynomailFunction(double x) => 8* x * x * x - 3 * x * x + 4 * x - 10;

        /// <summary>
        /// Function of third degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ThirdDegreePolynomailFunctionIntegral(double x) => 2 * x * x * x * x - x * x * x + 2 * x * x - 10 * x;

        /// <summary>
        /// Function of weight
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double WeightFunction(double x) => 1;

        /// <summary>
        /// Left rectangles compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <returns>Formula's value</returns>
        public double LeftRectangles(Func<double, double> function) => (sumY + function(left)) * delta;

        /// <summary>
        /// Right rectangles compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <returns>Formula's value</returns>
        public double RightRectangles(Func<double, double> function) => (sumY + function(right)) * delta;

        /// <summary>
        /// Middle rectangles compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <returns>Formula's value</returns>
        public double MiddleRectangles(Func<double, double> function) => delta * sumP;

        /// <summary>
        /// Trapezium compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <returns>Formula's value</returns>
        public double Trapezium(Func<double, double> function) => ((function(left) + function(right)) / 2 + sumY) * delta;

        /// <summary>
        /// Simpson's compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <returns>Formula's value</returns>
        public double Simpson(Func<double, double> function) => (4 * sumP + function(left) + function(right) + 2 * sumY) * delta / 6;

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program executes the approximate calculatation of integral's value with compound quadrature formulas.");
            Console.WriteLine();
            Process();
        }

        /// <summary>
        /// Executes program's process
        /// </summary>
        public void Process()
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

            Console.WriteLine("\nPlease, enter amount of intervlas.");
            var m = 0;
            var flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
            while (!flagM)
            {
                Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of intervlas.");
                flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
            }
            amountOfIntervals = m;

            delta = (right - left) / amountOfIntervals;

            var functions = new List<(Func<double, double>, string, Func<double, double>)>();
            functions.Add((ZeroDegreePolynomailFunction, zeroDegreePolynomailFunction, ZeroDegreePolynomailFunctionIntegral));
            functions.Add((FirstDegreePolynomailFunction, firstDegreePolynomailFunction, FirstDegreePolynomailFunctionIntegral));
            functions.Add((SecondDegreePolynomailFunction, secondDegreePolynomailFunction, SecondDegreePolynomailFunctionIntegral));
            functions.Add((ThirdDegreePolynomailFunction, thirdDegreePolynomailFunction, ThirdDegreePolynomailFunctionIntegral));

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine($"Integration interval: [{left}, {right}].");
                Console.WriteLine($"Amount of intervals = m: {amountOfIntervals}.");
                Console.WriteLine($"Delta = h: {delta}.");
                Console.WriteLine($"Function: {functions[i].Item2}.");
                Console.WriteLine($"Value of integral: {functions[i].Item3(right - left)}.");

                sumY = 0;
                sumP = functions[i].Item1(left);
                for (var j = 1; i < amountOfIntervals; i++)
                {
                    sumY += functions[i].Item1(left + delta * j);
                    sumP += functions[i].Item1(left + delta * j + (delta / 2));
                }
            }
        }
    }
}
