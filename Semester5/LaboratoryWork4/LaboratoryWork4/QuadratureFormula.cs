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

        private readonly string zeroDegreePolynomailFunction = "f(x) = 6";

        /// <summary>
        /// Function of zero degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunction(double x) => 6;

        private readonly string zeroDegreePolynomailFunctionIntegral = "J(x) = 6 * x";

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

        private readonly string firstDegreePolynomailFunctionIntegral = "J(x) = 2 * x ^ 2 + 8 * x";

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

        private readonly string secondDegreePolynomailFunctionIntegral = "J(x) = x ^ 3 - x ^ 2 + 7 * x";

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

        private readonly string thirdDegreePolynomailFunctionIntegral = "J(x) = 2 * x ^ 4 - x ^ 3 + 2 * x ^ 2 - 10 * x";

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
        /// <param name="integral">Function's integral</param>
        /// <returns>Formula's value</returns>
        public double LeftRectangles(Func<double, double> function, Func<double, double> integral)
        {
            double sum = 0;
            for (var i = 1; i <= amountOfIntervals; i++)
            {
                sum += function(left + (i - 1) * delta);
            }
            return sum * delta;
        }

        /// <summary>
        /// Right rectangles compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <param name="integral">Function's integral</param>
        /// <returns>Formula's value</returns>
        public double RightRectangles(Func<double, double> function, Func<double, double> integral)
        {
            double sum = 0;
            for (var i = 1; i <= amountOfIntervals; i++)
            {
                sum += function(left + delta + (i - 1) * delta);
            }
            return sum * delta;
        }

        /// <summary>
        /// Middle rectangles compound quadrature formula
        /// </summary>
        /// <param name="function">Given function</param>
        /// <param name="integral">Function's integral</param>
        /// <returns>Formula's value</returns>
        public double MiddleRectangles(Func<double, double> function, Func<double, double> integral)
        {
            double sum = 0;
            for (var i = 1; i <= amountOfIntervals; i++)
            {
                sum += function(left + delta / 2 + (i - 1) * delta);
            }
            return sum * delta;
        }

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

            var functions = new List<(Func<double, double>, string, Func<double, double>, string)>();
            functions.Add((ZeroDegreePolynomailFunction, zeroDegreePolynomailFunction, ZeroDegreePolynomailFunctionIntegral, zeroDegreePolynomailFunctionIntegral));
            functions.Add((FirstDegreePolynomailFunction, firstDegreePolynomailFunction, FirstDegreePolynomailFunctionIntegral, firstDegreePolynomailFunctionIntegral));
            functions.Add((SecondDegreePolynomailFunction, secondDegreePolynomailFunction, SecondDegreePolynomailFunctionIntegral, secondDegreePolynomailFunctionIntegral));
            functions.Add((ThirdDegreePolynomailFunction, thirdDegreePolynomailFunction, ThirdDegreePolynomailFunctionIntegral, thirdDegreePolynomailFunctionIntegral));

            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine($"Integration interval: [{left}, {right}].");
                Console.WriteLine($"Amount of intervals = m: {amountOfIntervals}.");
                Console.WriteLine($"Delta = h: {delta}.");
                Console.WriteLine($"Function: {functions[i].Item2}.");
                Console.WriteLine($"Integral: {functions[i].Item4}.");

            }
        }
    }
}
