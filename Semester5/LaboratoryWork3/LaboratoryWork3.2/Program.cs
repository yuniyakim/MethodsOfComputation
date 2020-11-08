using System;

namespace LaboratoryWork3._2
{
    public class Program
    {
        public static void Main()
        {
            var numericalDifferentiation = new NumericalDifferentiation();
            numericalDifferentiation.Start();
            var input = "";
            Console.WriteLine("\nDo you want to enter new left border, amount of nodes and partition? Y/N");
            input = Console.ReadLine();
            while (input != "Y" && input != "N")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }
            while (input == "Y")
            {
                numericalDifferentiation.Process(true);
                Console.WriteLine("\nDo you want to enter new left border, amount of nodes and partition? Y/N");
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
