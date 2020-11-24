using System;
using System.Collections.Generic;

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

        private readonly string someFunction = "e ^ x";

        /// <summary>
        /// Some function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SomeFunction(double x) => Math.Pow(Math.E, x);

        /// <summary>
        /// Some function's integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SomeFunctionIntegral(double x) => Math.Pow(Math.E, x);

        private readonly string anotherFunction = "-lnx";

        /// <summary>
        /// Another function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double AnotherFunction(double x) => -Math.Log(x);

        /// <summary>
        /// Another function's integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double AnotherFunctionIntegral(double x) => x - x*Math.Log(x);

        private readonly string zeroDegreePolynomailFunction = "6";

        /// <summary>
        /// Function of zero degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunction(double x) => 6;

        /// <summary>
        /// Function's of zero degree polynomial integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunctionIntegral(double x) => 6 * x;

        private readonly string firstDegreePolynomailFunction = "4 * x + 8";

        /// <summary>
        /// Function of first degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double FirstDegreePolynomailFunction(double x) => 4 * x + 8;

        /// <summary>
        /// Function's of first degree polynomial integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double FirstDegreePolynomailFunctionIntegral(double x) => 2 * x * x + 8 * x;

        private readonly string secondDegreePolynomailFunction = "3 * x ^ 2 - 2 * x + 7";

        /// <summary>
        /// Function of second degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SecondDegreePolynomailFunction(double x) => 3 * x * x - 2 * x + 7;

        /// <summary>
        /// Function's of second degree polynomial integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SecondDegreePolynomailFunctionIntegral(double x) => x * x * x - x * x + 7 * x;

        private readonly string thirdDegreePolynomailFunction = "8 * x ^ 3 - 3 * x ^ 2 + 4 * x - 10";

        /// <summary>
        /// Function of third degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ThirdDegreePolynomailFunction(double x) => 8* x * x * x - 3 * x * x + 4 * x - 10;

        /// <summary>
        /// Function's of third degree polynomial integral
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ThirdDegreePolynomailFunctionIntegral(double x) => 2 * x * x * x * x - x * x * x + 2 * x * x - 10 * x;

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
            functions.Add((SomeFunction, someFunction, SomeFunctionIntegral));
            functions.Add((AnotherFunction, anotherFunction, AnotherFunctionIntegral));
            functions.Add((ZeroDegreePolynomailFunction, zeroDegreePolynomailFunction, ZeroDegreePolynomailFunctionIntegral));
            functions.Add((FirstDegreePolynomailFunction, firstDegreePolynomailFunction, FirstDegreePolynomailFunctionIntegral));
            functions.Add((SecondDegreePolynomailFunction, secondDegreePolynomailFunction, SecondDegreePolynomailFunctionIntegral));
            functions.Add((ThirdDegreePolynomailFunction, thirdDegreePolynomailFunction, ThirdDegreePolynomailFunctionIntegral));

            Console.WriteLine();

            for (var i = 1; i < 6; i++)
            {
                var integral = functions[i].Item3(right) - functions[i].Item3(left);
                Console.WriteLine($"Integration interval: [{left}, {right}].");
                Console.WriteLine($"Amount of intervals = m: {amountOfIntervals}.");
                Console.WriteLine($"Delta = h: {delta}.");
                Console.WriteLine($"Function = f(x): {functions[i].Item2}.");
                Console.WriteLine($"Value of integral = J: {integral}.");
                Console.WriteLine();

                sumY = 0;
                sumP = functions[i].Item1(left + (delta / 2));
                for (double j = 1; j < amountOfIntervals; j++)
                {
                    sumY += functions[i].Item1(left + delta * j);
                    sumP += functions[i].Item1(left + delta * j + (delta / 2));
                }

                Console.WriteLine($"LEFT RECTANGLES FORMULA");
                Console.WriteLine($"Formula's value = J(h): {LeftRectangles(functions[i].Item1)}");
                Console.WriteLine($"Absolute actual error = |J - J(h)|: {Math.Abs(integral - LeftRectangles(functions[i].Item1))}");
                if (i == 0)
                {
                    Console.WriteLine($"Theoretical error: {Math.Pow(delta, 1) * (right - left) * Math.Abs(functions[i].Item1(right)) / 2}");
                }
                Console.WriteLine();

                Console.WriteLine($"RIGHT RECTANGLES FORMULA");
                Console.WriteLine($"Formula's value = J(h): {RightRectangles(functions[i].Item1)}");
                Console.WriteLine($"Absolute actual error = |J - J(h)|: {Math.Abs(integral - RightRectangles(functions[i].Item1))}");
                if (i == 0)
                {
                    Console.WriteLine($"Theoretical error: {Math.Pow(delta, 1) * (right - left) * Math.Abs(functions[i].Item1(right)) / 2}");
                }
                Console.WriteLine();

                Console.WriteLine($"MIDDLE RECTANGLES FORMULA");
                Console.WriteLine($"Formula's value = J(h): {MiddleRectangles(functions[i].Item1)}");
                Console.WriteLine($"Absolute actual error = |J - J(h)|: {Math.Abs(integral - MiddleRectangles(functions[i].Item1))}");
                if (i == 0)
                {
                    Console.WriteLine($"Theoretical error: {Math.Pow(delta, 2) * (right - left) * Math.Abs(functions[i].Item1(right)) / 24}");
                }
                Console.WriteLine();

                Console.WriteLine($"TRAPEZIUM FORMULA");
                Console.WriteLine($"Formula's value = J(h): {Trapezium(functions[i].Item1)}");
                Console.WriteLine($"Absolute actual error = |J - J(h)|: {Math.Abs(integral - Trapezium(functions[i].Item1))}");
                if (i == 0)
                {
                    Console.WriteLine($"Theoretical error: {Math.Pow(delta, 2) * (right - left) * Math.Abs(functions[i].Item1(right)) / 12}");
                }
                Console.WriteLine();

                Console.WriteLine($"SIMPSON'S FORMULA");
                Console.WriteLine($"Formula's value = J(h): {Simpson(functions[i].Item1)}");
                Console.WriteLine($"Absolute actual error = |J - J(h)|: {Math.Abs(integral - Simpson(functions[i].Item1))}");
                if (i == 0)
                {
                    Console.WriteLine($"Theoretical error: {Math.Pow(delta, 4) * (right - left) * Math.Abs(functions[i].Item1(right)) / 2880}");
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}
