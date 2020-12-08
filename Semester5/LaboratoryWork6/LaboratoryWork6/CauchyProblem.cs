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
        /// Calculates approximate value of solution using Runge-Kutta in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> RungeKutta(List<double> points)
        {
            var results = new List<double>();
            for (var i = 0; i < points.Count - 3; i++)
            {
                var yi = (i == 0 ? y0 : results[i - 1]);
                var xi = points[i + 2];
                var k1 = h * (-yi + Math.Sin(xi));
                var k2 = h * (-(yi + k1 / 2) + Math.Sin(xi + h / 2));
                var k3 = h * (-(yi + k2 / 2) + Math.Sin(xi + h / 2));
                var k4 = h * (-(yi + k3) + Math.Sin(xi + h));
                results.Add(yi + (k1 + 2 * k2 + 2 * k3 + k4) / 6);
            }

            return results;
        }

        /// <summary>
        /// Calculates approximate value of solution using Euler in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> Euler(List<double> points)
        {
            var results = new List<double>();
            for (var i = 0; i < points.Count - 3; i++)
            {
                var yi = (i == 0 ? y0 : results[i - 1]);
                var xi = points[i + 2];
                results.Add(yi + h * (-yi + Math.Sin(xi)));
            }

            return results;
        }

        /// <summary>
        /// Calculates approximate value of solution using Euler1 in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> Euler1(List<double> points)
        {
            var results = new List<double>();
            for (var i = 0; i < points.Count - 3; i++)
            {
                var yi = (i == 0 ? y0 : results[i - 1]);
                var xi = points[i + 2];
                results.Add(yi + h * (-(yi + h * (-yi + Math.Sin(xi)) / 2 ) + Math.Sin(xi + h / 2)));
            }

            return results;
        }

        /// <summary>
        /// Calculates approximate value of solution using Euler2 in given points
        /// </summary>
        /// <param name="points">Given points</param>
        /// <returns>List with value of solution in points</returns>
        public List<double> Euler2(List<double> points)
        {
            var results = new List<double>();
            for (var i = 0; i < points.Count - 3; i++)
            {
                var yi = (i == 0 ? y0 : results[i - 1]);
                var xi = points[i + 2];
                var xi1 = points[i + 3];
                results.Add(yi + h * ((-yi + Math.Sin(xi)) + (-(yi + h * (-yi + Math.Sin(xi))) + Math.Sin(xi1))) / 2);
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

            var points = new List<double>();
            for (var i = -2; i <= n; i++)
            {
                points.Add(x0 + i * h);
            }

            var solutions = new List<double>();
            Console.WriteLine($"THE EXACT SOLUTION");
            Console.WriteLine(String.Format("{0,-25} | {1,0}", "xk", "y(xk)"));
            foreach (var point in points)
            {
                var solution = Solution(point);
                solutions.Add(solution);
                Console.WriteLine(String.Format("{0,-25} | {1,0}", point, solution));
            }

            Console.WriteLine();
            Console.WriteLine();

            var taylor = Taylor(points);
            Console.WriteLine($"TAYLOR");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], taylor[i], Math.Abs(solutions[i] - taylor[i])));
            }

            Console.WriteLine();
            Console.WriteLine();

            var adams = Adams(points, taylor);
            Console.WriteLine($"ADAMS");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                if (i < 5)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], adams[i - 5], Math.Abs(solutions[i] - adams[i - 5])));
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            var rungeKutta = RungeKutta(points);
            Console.WriteLine($"RUNGE-KUTTA");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                if (i < 3)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], rungeKutta[i - 3], Math.Abs(solutions[i] - rungeKutta[i - 3])));
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            var euler = Euler(points);
            Console.WriteLine($"EULER");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                if (i < 3)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], euler[i - 3], Math.Abs(solutions[i] - euler[i - 3])));
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            var euler1 = Euler1(points);
            Console.WriteLine($"EULER");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                if (i < 3)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], euler1[i - 3], Math.Abs(solutions[i] - euler1[i - 3])));
                }
            }

            Console.WriteLine();
            Console.WriteLine();

            var euler2 = Euler2(points);
            Console.WriteLine($"EULER");
            Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", "xk", "yN(xk)", "|y(xk) - yN(xk)|"));
            for (var i = 0; i < points.Count; i++)
            {
                if (i < 3)
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], "---", "---"));
                }
                else
                {
                    Console.WriteLine(String.Format("{0,-25} | {1,-25} | {2,0}", points[i], euler2[i - 3], Math.Abs(solutions[i] - euler2[i - 3])));
                }
            }

            Console.WriteLine();
        }
    }
}