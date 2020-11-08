using System;

namespace LaboratoryWork2_2
{
    public class Program
    {
        public static void Main()
        {
            var newtonInterpolation = new NewtonInterpolation();
            newtonInterpolation.Start();
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
                newtonInterpolation.Process(true);
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