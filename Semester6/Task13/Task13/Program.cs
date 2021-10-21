using System;

namespace Task13
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}|{3,-25}", "N", "Uniform density", "Linear density", "Area"));
            for (var i = 2; i < 6; i++)
            {
                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}|{3,-25}", $"{Math.Pow(10, i)}",
                    $"{Math.Abs(1 - MonteCarloMethod.MonteCarlo((int)Math.Pow(10, i), 0, Math.PI / 2, "uniform"))}",
                    $"{Math.Abs(1 - MonteCarloMethod.MonteCarlo((int)Math.Pow(10, i), 0, Math.PI / 2, "linear"))}",
                    $"{Math.Abs(1 - MonteCarloMethod.MonteCarloArea((int)Math.Pow(10, i), 0, Math.PI / 2))}"));
            }
        }
    }
}
