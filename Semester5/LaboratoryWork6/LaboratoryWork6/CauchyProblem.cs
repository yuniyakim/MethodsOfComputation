using System;
using System.Collections.Generic;

namespace LaboratoryWork6
{
    /// <summary>
    /// Cauchy problem
    /// </summary>
    public class CauchyProblem
    {
        private double x0 = 0;
        private double y0 = 1;
        private double h = 0.1;
        private int n = 10;
        private readonly string equation = "y'(x) = - y(x) + sin(x), y(0) = 1";

        /// <summary>
        /// Solution
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double Solution(double x) => (3 * Math.Exp(-x) + Math.Sin(x) - Math.Cos(x)) / 2;

        /// <summary>
        /// Calculates the factorial of number
        /// </summary>
        /// <param name="number">Given number</param>
        /// <returns>Number's factorial</returns>
        public int Factorial(int number)
        {
            if (number < 0)
            {
                throw new ArgumentException();
            }

            var result = 1;
            for (var i = number; i > 0; i--)
            {
                result *= i;
            }
            return result;
        }

        /// <summary>
        /// Calculates approximate value of solution using Taylor in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> Taylor(List<double> points)
        {
            var counter = 0;
            var derivatives = new List<double>();
            derivatives.Add(y0);
            for (var i = 1; counter < 4; i++)
            {
                derivatives.Add(-derivatives[i - 1]);
                switch (i % 4)
                {
                    case 1:
                        {
                            derivatives[i] += Math.Sin(x0);
                            break;
                        }
                    case 2:
                        {
                            derivatives[i] += Math.Cos(x0);
                            break;
                        }
                    case 3:
                        {
                            derivatives[i] -= Math.Sin(x0);
                            break;
                        }
                    case 0:
                        {
                            derivatives[i] -= Math.Cos(x0);
                            break;
                        }
                }
                if (derivatives[i] != 0)
                {
                    counter += 1;
                }
            }

            Func<double, double> taylor = x =>
            {
                double result = 0;
                for (var i = 0; i < derivatives.Count; i++)
                {
                    result += derivatives[i] * Math.Pow((x - x0), i) / Factorial(i);
                }
                return result;
            };

            var values = new List<double>();
            foreach (var point in points)
            {
                values.Add(taylor(point));
            }
            return values;
        }

        /// <summary>
        /// Calculates approximate value of solution using Adams in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <param name="values">known values</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> Adams(List<double> points, List<double> values)
        {
            var table = new double[points.Count, 6];
            for (var i = 0; i < 5; i++)
            {
                table[i, 0] = values[i];
                table[i, 1] = h * (-table[i, 0] + Math.Sin(points[i]));
            }

            var helper = 5;
            for (var i = 1; i < 5; i++)
            {
                for (var j = 0; j < helper; j++)
                {
                    table[j, i + 1] = table[j + 1, i] - table[j, i];
                }
                helper--;
            }

            var results = new List<double>();
            for (var i = 5; i < points.Count; i++)
            {
                table[i, 0] = table[i - 1, 0] + table[i - 1, 1] + table[i - 2, 2] / 2 + 5 * table[i - 3, 3] / 12 +
                    3 * table[i - 4, 4] / 8 + 251 * table[i - 5, 5] / 720;
                results.Add(table[i, 0]);
                if (i != points.Count)
                {
                    table[i, 1] = h * (-table[i, 0] + Math.Sin(points[i]));
                    for (var j = 0; j < 4; j++)
                    {
                        table[i - j - 1, j + 2] = table[i - j, j + 1] - table[i - j - 1, j + 1];
                    }
                }
            }

            return results;
        }

        /// <summary>
        /// Starts program
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Hello!");
            Console.WriteLine("This program executes the numerical solution of the Cauchy problem for ordinary differential equation of the first order.");
            Console.WriteLine($"Equation: {equation}.");
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
                Console.WriteLine("\nPlease, enter step.");
                double inputH = 0;
                var flagH = double.TryParse(Console.ReadLine(), out inputH);
                while (!flagH)
                {
                    Console.WriteLine($"Please, enter CORRECT (double, greater than 0) step.");
                    flagH = double.TryParse(Console.ReadLine(), out inputH);
                }
                h = inputH;

                Console.WriteLine("\nPlease, enter amount of points.");
                var inputN = 0;
                var flagN = int.TryParse(Console.ReadLine(), out inputN) && inputN > 4;
                while (!flagN)
                {
                    Console.WriteLine($"Please, enter CORRECT (int, greater than 0) amount of points.");
                    flagN = int.TryParse(Console.ReadLine(), out inputN) && inputN > 4;
                }
                n = inputN;
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
                Console.WriteLine("\nPlease, enter step.");
                double inputH = 0;
                var flagH = double.TryParse(Console.ReadLine(), out inputH);
                while (!flagH)
                {
                    Console.WriteLine($"Please, enter CORRECT (double, greater than 0) step.");
                    flagH = double.TryParse(Console.ReadLine(), out inputH);
                }
                h = inputH;

                Console.WriteLine("\nPlease, enter amount of points.");
                var inputN = 0;
                var flagN = int.TryParse(Console.ReadLine(), out inputN) && inputN > 4;
                while (!flagN)
                {
                    Console.WriteLine($"Please, enter CORRECT (int, greater than 0) amount of points.");
                    flagN = int.TryParse(Console.ReadLine(), out inputN) && inputN > 4;
                }
                n = inputN;
            }

            Console.WriteLine();

            Console.WriteLine($"h = {h}.");
            Console.WriteLine($"N = {n}.");
            Console.WriteLine();

            var points1 = new List<double>();
            for (var i = -2; i <= n; i++)
            {
                points1.Add(x0 + i * h);
            }
            var points3 = points1.GetRange(3, points1.Count - 3);

            var solutions = new List<double>();
            Console.WriteLine($"THE EXACT SOLUTION");
            Console.WriteLine(String.Format("{0,-25} | {1,0}", "xk", "y(xk)"));
            foreach (var point in points1)
            {
                var solution = Solution(point);
                solutions.Add(solution);
                Console.WriteLine(String.Format("{0,-25} | {1,0}", point, solution));
            }

            Console.WriteLine();
            Console.WriteLine();

            var taylor = Taylor(points1);
            Console.WriteLine($"TAYLOR");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points1.Count; i++)
            {
                Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points1[i], taylor[i], Math.Abs(solutions[i] - taylor[i])));
            }

            Console.WriteLine();
            Console.WriteLine();

            var adams = Adams(points1, taylor);
            Console.WriteLine($"ADAMS");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points1.Count; i++)
            {
                if (i < 5)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points1[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points1[i], adams[i - 5], Math.Abs(solutions[i] - adams[i - 5])));
                }
            }

            Console.WriteLine();
            Console.WriteLine();
        }
    }
}