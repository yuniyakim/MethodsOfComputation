﻿using System;
using System.Collections.Generic;

namespace LaboratoryWork5
{
    /// <summary>
    /// Quadrature formulas with highest algebraic degree of accuracy
    /// </summary>
    public class QuadratureFormulaHADoA
    {
        private double left = -1; // A
        private double right = 1; // B
        private int amountOfIntervals = 5; // m
        private double delta; // h
        private double sumP;
        private double mLeft = -1;
        private double mRight = 1;
        private double mAmountOfNodes = 7; // N

        private readonly string function = "sin(x)";

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double Function(double x) => 2.175 * Math.Pow(x, 5) - 3.267 * Math.Pow(x, 2) + 6.321;
        private const double integral = 0.239812;

        private readonly string weightFunction = "-ln(x)";

        /// <summary>
        /// Weight function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of weight function</returns>
        private double WeightFunction(double x) => 1 / Math.Sqrt(1 - x * x);

        private readonly string mFunction = "cos(3x) / (0.3 + x^2)";

        /// <summary>
        /// Meler function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double MFunction(double x) => 2.175 * Math.Pow(x, 5) - 3.267 * Math.Pow(x, 2) + 6.321;

        private readonly string mWeightFunction = "1 / sqrt(1 - x^2)";

        /// <summary>
        /// Meler Weight function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of weight function</returns>
        private double MWeightFunction(double x) => 1 / Math.Sqrt(1 - x * x);
        private const double mIntegral = 0.5771;

        //private readonly string function = "1";
        //private double Function(double x) => 1;
        //private double Integral(double x) => x;
        //private const double integral = 1;

        //private readonly string function = "x";
        //private double Function(double x) => x;
        //private double Integral(double x) => x * x;
        //private const double integral = 0.25;

        //private readonly string function = "x ^ 2";
        //private double Function(double x) => x * x;
        //private double Integral(double x) => x * x * x;
        //private const double integral = 0.11111;

        //private readonly string function = "x ^ 3";
        //private double Function(double x) => x * x * x;
        //private double Integral(double x) => x * x * x * x;
        //private const double integral = 0.0625;

        /// <summary>
        /// Moment's 0 function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of moment's 0 function</returns>
        private double Moment0(double x) => WeightFunction(x);

        /// <summary>
        /// Moment's 1 function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of moment's 1 function</returns>
        private double Moment1(double x) => WeightFunction(x) * x;

        /// <summary>
        /// Moment's 2 function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of moment's 2 function</returns>
        private double Moment2(double x) => WeightFunction(x) * x * x;

        /// <summary>
        /// Moment's 3 function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of moment's 3 function</returns>
        private double Moment3(double x) => WeightFunction(x) * x * x * x;

        /// <summary>
        /// Middle rectangles compound quadrature formula
        /// </summary>
        /// <returns>Formula's value</returns>
        public double MiddleRectangles() => delta * sumP;

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
                //Console.WriteLine("Please, enter the left border of interval.");
                //Console.WriteLine("NOTE: doubles are being entered with dot, NOT comma.");
                //double A = 0;
                //var flagA = double.TryParse(Console.ReadLine(), out A);
                //while (!flagA)
                //{
                //    Console.WriteLine("Please, enter the CORRECT (double) left border of interval.");
                //    flagA = double.TryParse(Console.ReadLine(), out A);
                //}
                //left = A;

                //Console.WriteLine("\nPlease, enter the right border of interval.");
                //double B = 0;
                //var flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                //while (!flagB)
                //{
                //    Console.WriteLine($"Please, enter the CORRECT (double, greater than A = {A}) right border of interval.");
                //    flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                //}
                //right = B;

                Console.WriteLine("\nPlease, enter amount of intervlas.");
                var m = 0;
                var flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                while (!flagM)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of intervlas.");
                    flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                }
                amountOfIntervals = m;

                Console.WriteLine("\nPlease, enter amount of nodes.");
                var l = 0;
                var flagL = int.TryParse(Console.ReadLine(), out l) && l > 0;
                while (!flagL)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of nodes.");
                    flagL = int.TryParse(Console.ReadLine(), out l) && l > 0;
                }
                mAmountOfNodes = l;
            }

            Process(false);
        }

        /// <summary>
        /// Executes program's process
        /// </summary>
        /// <param name="input">Shows if input is needed</param>
        public void Process(bool input)
        {
            if (input)
            {
                //Console.WriteLine("Please, enter the left border of interval.");
                //Console.WriteLine("NOTE: doubles are being entered with dot, NOT comma.");
                //double A = 0;
                //var flagA = double.TryParse(Console.ReadLine(), out A);
                //while (!flagA)
                //{
                //    Console.WriteLine("Please, enter the CORRECT (double) left border of interval.");
                //    flagA = double.TryParse(Console.ReadLine(), out A);
                //}
                //left = A;

                //Console.WriteLine("\nPlease, enter the right border of interval.");
                //double B = 0;
                //var flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                //while (!flagB)
                //{
                //    Console.WriteLine($"Please, enter the CORRECT (double, greater than A = {A}) right border of interval.");
                //    flagB = double.TryParse(Console.ReadLine(), out B) && B > A;
                //}
                //right = B;

                Console.WriteLine("\nPlease, enter amount of intervlas.");
                var m = 0;
                var flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                while (!flagM)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of intervlas.");
                    flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                }
                amountOfIntervals = m;

                Console.WriteLine("\nPlease, enter amount of nodes.");
                var l = 0;
                var flagL = int.TryParse(Console.ReadLine(), out l) && l > 0;
                while (!flagL)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of nodes.");
                    flagL = int.TryParse(Console.ReadLine(), out l) && l > 0;
                }
                mAmountOfNodes = l;
            }

            Console.WriteLine();

            Console.WriteLine($"Integration interval: [{left}, {right}].");
            Console.WriteLine($"Amount of intervals = m: {amountOfIntervals}.");
            Console.WriteLine($"Function = f(x): {function}.");
            Console.WriteLine($"Weight function = p(x): {weightFunction}.");

            Console.WriteLine();
            Console.WriteLine();

            double result = 0;
            delta = (right - left) / amountOfIntervals;

            for (var i = 0; i <= amountOfIntervals - 1; i++)
            {
                var a = left + i * delta;
                var b = left + (i + 1) * delta;
                var argument1 = delta * (1 / Math.Sqrt(3)) / 2 + a + delta / 2;
                var argument2 = delta * (-1 / Math.Sqrt(3)) / 2 +a + delta / 2;
                result += (b - a) * (Function(argument1) * WeightFunction(argument1) + Function(argument2) * WeightFunction(argument2)) / 2;
            }
            Console.WriteLine("GAUSS COMPOUND QUARATURE FORMULA");
            Console.WriteLine($"INTEGRAL'S VALUE with Gauss compound quadrature formula: {result}.");
            Console.WriteLine($"Error: {Math.Abs(result - integral)}.");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine("GAUSS TYPE QUARATURE FORMULA");

            delta = (right - left) / 1000000;

            var moments = new List<(Func<double, double>, double)>();
            moments.Add((Moment0, 0));
            moments.Add((Moment1, 0));
            moments.Add((Moment2, 0));
            moments.Add((Moment3, 0));

            for (var i = 0; i <= 3; i++)
            {
                sumP = moments[i].Item1(left + (delta / 2));
                for (double j = 1; j < 1000000; j++)
                {
                    sumP += moments[i].Item1(left + delta * j + (delta / 2));
                }
                moments[i] = (moments[i].Item1, MiddleRectangles());
                Console.WriteLine($"Moment {i}: {moments[i].Item2}.");
            }

            var a1 = (moments[0].Item2 * moments[3].Item2 - moments[2].Item2 * moments[1].Item2) / 
                (moments[1].Item2 * moments[1].Item2 - moments[2].Item2 * moments[0].Item2);
            var a2 = (moments[2].Item2 * moments[2].Item2 - moments[3].Item2 * moments[1].Item2) /
                (moments[1].Item2 * moments[1].Item2 - moments[2].Item2 * moments[0].Item2);
            Console.WriteLine($"Orthogonal polynomial: x^2 + {a1} * x + {a2}.");

            double root1 = (-a1 + Math.Sqrt(a1 * a1 - 4 * a2)) / 2;
            double root2 = (-a1 - Math.Sqrt(a1 * a1 - 4 * a2)) / 2;
            if (root1 <= left || root1 >= right || root2 <= left || root2 >= right)
            {
                throw new ArithmeticException("Invalid roots.");
            }
            Console.WriteLine($"Nodes of quadrature formula: {root1}, {root2}.");

            double coefficient1 = (moments[1].Item2 - root2 * moments[0].Item2) / (root1 - root2);
            double coefficient2 = (moments[1].Item2 - root1 * moments[0].Item2) / (root2 - root1);
            if (coefficient1 + coefficient2 - moments[0].Item2 > Math.Pow(10, -15) && coefficient1 < 0 && coefficient2 < 0)
            {
                throw new ArithmeticException("Invalid coefficients");
            }
            Console.WriteLine($"Coefficients of quadrature formula: {coefficient1}, {coefficient2}.");

            result = coefficient1 * Function(root1) + coefficient2 * Function(root2);
            Console.WriteLine($"INTEGRAL'S VALUE with Gauss type quadrature formula: {result}.");
            Console.WriteLine($"Error: {Math.Abs(result - integral)}.");

            Console.WriteLine();
            Console.WriteLine();

            Console.WriteLine($"Integration interval: [{mLeft}, {mRight}].");
            Console.WriteLine($"Amount of nodes = N: {mAmountOfNodes}.");
            Console.WriteLine($"Function = f(x): {mFunction}.");
            Console.WriteLine($"Weight function = p(x): {mWeightFunction}.");

            Console.WriteLine();
            Console.WriteLine();

            double sum = 0;
            for (var i = 1; i <= mAmountOfNodes; i++)
            {
                sum += MFunction(Math.Cos((2 * i - 1) * Math.PI / (2 * mAmountOfNodes)));
            }
            result = Math.PI * sum / mAmountOfNodes;

            Console.WriteLine("MELER QUARATURE FORMULA");
            Console.WriteLine($"INTEGRAL'S VALUE with Meler quadrature formula: {result}.");
            Console.WriteLine($"Error: {Math.Abs(result - mIntegral)}.");
        }
    }
}
