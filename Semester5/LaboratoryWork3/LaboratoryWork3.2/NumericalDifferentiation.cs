using System;
using System.Collections.Generic;

namespace LaboratoryWork3._2
{
    public class NumericalDifferentiation
    {
        private double left = 0; // a
        private int amountOfNodes = 0; // m
        private double partition = 0; // h

        /// <summary>
        /// Function
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double Function(double x) => Math.Exp(1.5 * x);

        /// <summary>
        /// First derivative
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of first derivative</returns>
        private double FirstDerivative(double x) => 1.5 * Math.Exp(1.5 * x);

        /// <summary>
        /// Second derivative
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of second derivative</returns>
        private double SecondDerivative(double x) => 2.25 * Math.Exp(1.5 * x);

        /// <summary>
        /// Starts the main process
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

                Console.WriteLine("\nPlease, enter amount of nodes.");
                var m = 0;
                var flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                while (!flagM)
                {
                    Console.WriteLine($"Please, enter the CORRECT (int, greater than 0) amount of nodes.");
                    flagM = int.TryParse(Console.ReadLine(), out m) && m > 0;
                }
                amountOfNodes = m;

                Console.WriteLine("\nPlease, enter desired partition.");
                double part = 0;
                var flagPart = double.TryParse(Console.ReadLine(), out part) && part > 0;
                while (!flagPart)
                {
                    Console.WriteLine($"Please, enter the CORRECT (double, greater than 0) desired partition.");
                    flagPart = double.TryParse(Console.ReadLine(), out part) && part > 0;
                }
                partition = part;
            }

            var table = new List<List<double>>();
            for (var i = 0; i <= amountOfNodes; i++)
            {
                table.Add(new List<double>());
                for (var j = 0; j < 6; j++)
                {
                    table[i].Add(0);
                }
            }

            for (var i = 0; i <= amountOfNodes; i++)
            {
                var point = left + i * partition;
                table[i][0] = point;
                table[i][1] = Function(point);
                if (i == 0)
                {
                    table[i][2] = (-3 * Function(point) + 4 * Function(point + partition) - Function(point + 2 * partition)) / (2 * partition);
                }
                else if (i == amountOfNodes)
                {
                    table[i][2] = (3 * Function(point) - 4 * Function(point - partition) + Function(point - 2 * partition)) / (2 * partition);
                }
                else
                {
                    table[i][2] = (Function(point + partition) - Function(point - partition)) / (2 * partition);
                    table[i][4] = (Function(point + partition) - 2 * Function(point) + Function(point - partition)) / (partition * partition);
                    table[i][5] = Math.Abs(SecondDerivative(point) - table[i][4]);
                }
                table[i][3] = Math.Abs(FirstDerivative(point) - table[i][2]);
            }

            Console.WriteLine(String.Format("|{1,25}|{1,25}|{2,25}|{3,25}|{4,25}|{5,25}|", "xi", "f(xi)", "f'(xi)nd", "f'(xi)e - f'(xi)nd", "f''(xi)nd", "f''(xi)e - f''(xi)nd"));
            for (var i = 0; i <= amountOfNodes; i++)
            {
                Console.WriteLine(String.Format("|{0,25}|{1,25}|{2,25}|{3,25}|{4,25}|{5,25}|", table[i][0], table[i][1], table[i][2], table[i][3], table[i][4], table[i][5]));
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program solves the problem of numeric differentiation.");
            Console.WriteLine("Option 5.");
            Console.WriteLine();

            Process(true);
        }
    }
}
