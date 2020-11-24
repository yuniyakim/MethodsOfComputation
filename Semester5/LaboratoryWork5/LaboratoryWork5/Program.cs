using System;

namespace LaboratoryWork5
{
    public class Program
    {
        public static void Main()
        {
            var quadratureFormula = new QuadratureFormulaHADoA();
            quadratureFormula.Start();
            var input = "";
            Console.WriteLine("\nDo you want to enter new values of left border, right border and amount of intervals? Y/N");
            input = Console.ReadLine();
            while (input != "Y" && input != "N" && input != "y" && input != "n")
            {
                Console.WriteLine("Please, enter Y or N.");
                input = Console.ReadLine();
            }
            while (input == "Y" || input == "y")
            {
                quadratureFormula.Process(true);
                Console.WriteLine("\nDo you want to enter new values of left border, right border and amount of intervals? Y/N");
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
