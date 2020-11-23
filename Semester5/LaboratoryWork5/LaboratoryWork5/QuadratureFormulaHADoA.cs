using System;
using System.Collections.Generic;

namespace LaboratoryWork5
{
    /// <summary>
    /// Quadrature formulas with highest algebraic degree of accuracy
    /// </summary>
    public class QuadratureFormulaHADoA
    {
        private double left = 0; // A
        private double right = 1; // B
        private int amountOfIntervals = 100; // m
        private double delta; // h
        private double mLeft = -1;
        private double mRight = 1;
        private double mAmountOfNodes = 7; // N

        private readonly string function = "sin(x)";

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double Function(double x) => Math.Sin(x); 
        
        private readonly string weightFunction = "-ln(x)";

        /// <summary>
        /// Weight function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of weight function</returns>
        private double WeightFunction(double x) => - Math.Log(x);

        private readonly string mFunction = "cos(3x) / (0,3 + x^2)";

        /// <summary>
        /// Meler function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double MFunction(double x) => (Math.Cos(3 * x)) / (0.3 + x * x);

        private readonly string mWeightFunction = "1 / sqrt(1 - x)";

        /// <summary>
        /// Meler Weight function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of weight function</returns>
        private double MWeightFunction(double x) => 1 / Math.Sqrt(1 - x);

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
            Console.WriteLine("This program executes the approximate calculatation of integral's value with quadrature formulas with highest algebraic degree of accuracy.");
            Console.WriteLine("Option 5.");
            Console.WriteLine();

            Console.WriteLine("Do you want to enter parameters? Y/N");
            Console.WriteLine("NOTE: if N, program will use default values of parameters.");

            var input = Console.ReadLine();
            while (input != "Y" && input != "y" && input != "N" && input != "n")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }

            if (input == "Y" || input == "y")
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
            }

            Process(input == "Y" || input == "y");
        }

        /// <summary>
        /// Executes program's process
        /// </summary>
        /// <param name="input">Shows if input is needed</param>
        public void Process(bool input)
        {
            if (input)
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
            }

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
