using System;

namespace LaboratoryWork3
{
    public class Program
    {
        public static void Main()
        {
            var lagrangeInterpolation = new NewtonInterpolation();
            lagrangeInterpolation.Start();
            var input = "";
            Console.WriteLine("\nDo you want to enter new values of the power of polynomial and point? Y/N");
            input = Console.ReadLine();
            while (input != "Y" && input != "N")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }
            while (input == "Y")
            {
                lagrangeInterpolation.Process(true);
                Console.WriteLine("\nDo you want to enter new values of the power of polynomial and point? Y/N");
                input = Console.ReadLine();
                while (input != "Y" && input != "N")
                {
                    Console.WriteLine("Please, enter Y or N.");
                    input = Console.ReadLine();
                }
            }
        }
    }
}