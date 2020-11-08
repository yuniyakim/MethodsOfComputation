using System;

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

        /// <summary>
        /// Function of zero degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ZeroDegreePolynomailFunction(double x) => 6;

        /// <summary>
        /// Function of first degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double FirstDegreePolynomailFunction(double x) => 4 * x + 8;

        /// <summary>
        /// Function of second degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double SecondDegreePolynomailFunction(double x) => 3 * x * x - 2 * x + 7;

        /// <summary>
        /// Function of third degree polynomial
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double ThirdDegreePolynomailFunction(double x) => x * x * x - 2 * x * x + 4 * x - 10;

        /// <summary>
        /// Function of weight
        /// </summary>
        /// <param name="x">Argument</param>
        /// <returns>Value of function</returns>
        private double WeightFunction(double x) => 1;

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
            amountOfIntervals = m - 1;


        }
    }
}
