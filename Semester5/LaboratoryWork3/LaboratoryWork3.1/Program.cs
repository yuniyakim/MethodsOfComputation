using System;

namespace LaboratoryWork3_1
{
    public class Program
    {
        public static void Main()
        {
            var inverseInterpolation = new InverseInterpolation();
            inverseInterpolation.Start();
            var input = "";
            Console.WriteLine("\nDo you want to enter new value, power of polynomial and accuracy? Y/N");
            input = Console.ReadLine();
            while (input != "Y" && input != "N")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }
            while (input == "Y")
            {
                inverseInterpolation.Process(true);
                Console.WriteLine("\nDo you want to enter new value, power of polynomial and accuracy? Y/N");
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