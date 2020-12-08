using System;

namespace LaboratoryWork6
{
    public class Program
    {
        public static void Main()
        {
            var cauchyProblem = new CauchyProblem();
            cauchyProblem.Start();
            var input = "";
            Console.WriteLine("\nDo you want to enter new values of step and amount of points? Y/N");
            input = Console.ReadLine();
            while (input != "Y" && input != "N" && input != "y" && input != "n")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }
            while (input == "Y" || input == "y")
            {
                cauchyProblem.Process(true);
                Console.WriteLine("\nDo you want to enter new values of step and amount of points? Y/N");
                input = Console.ReadLine();
                while (input != "Y" && input != "N" && input != "y" && input != "n")
                {
                    Console.WriteLine("Please, enter Y or N.");
                    input = Console.ReadLine();
                }
            }
        }
    }
}
