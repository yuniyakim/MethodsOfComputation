using System;
using System.Collections.Generic;
using System.Linq;
using DotNumerics.LinearAlgebra;

namespace Task2
{
    /// <summary>
    /// LU decomposition of a matrix
    /// </summary>
    public class LUDecomposition
    {
        private Matrix matrix;

        public int size { get; private set; }

        public LUDecomposition() { }

        /// <summary>
        /// LU decmposition's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        public LUDecomposition(Matrix matrix)
        {
            this.matrix = matrix;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Calculates L and U matrices
        /// </summary>
        /// <returns>L and U matrices</returns>
        public (Matrix, Matrix) CalculateLUMatrices()
        {
            var lMatrix = new Matrix(size);
            var uMatrix = new Matrix(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (i <= j)
                    {
                        lMatrix[i, j] = (i == j) ? 1 : 0;
                        double sum = 0;
                        for (var k = 0; k < i -1; k++)
                        {
                            sum += lMatrix[i, k] * uMatrix[k, j];
                        }
                        uMatrix[i, j] = matrix[i, j] - sum;
                    }
                    else
                    {
                        uMatrix[i, j] = 0;
                        double sum = 0;
                        for (var k = 0; k < j - 1; k++)
                        {
                            sum += lMatrix[i, k] * uMatrix[k, j];
                        }
                        lMatrix[i, j] = (matrix[i, j] - sum) / lMatrix[i, i];
                    }
                }
            }
            return (lMatrix, uMatrix);
        }

        public void Start()
        {
            //var equations = new List<(Matrix, Vector, Vector)>();
            //var matrix1 = new Matrix(2, 2);
            //matrix1[0, 0] = -400.6;
            //matrix1[0, 1] = 199.8;
            //matrix1[1, 0] = 1198.8;
            //matrix1[1, 1] = -600.4;
            //var vector1 = new Vector(new double[] { 200, -600 });
            //var exactSolution1 = new Vector(new double[] { -0.2, 0.6 });
            //equations.Add((matrix1, vector1, exactSolution1));

            //var matrix2 = new Matrix(2, 2);
            //matrix2[0, 0] = 1;
            //matrix2[0, 1] = 0.99;
            //matrix2[1, 0] = 0.99;
            //matrix2[1, 1] = 0.98;
            //var vector2 = new Vector(new double[] { 2, 2 });
            //var exactSolution2 = new Vector(new double[] { 200, -200 });
            //equations.Add((matrix2, vector2, exactSolution2));

            //var matrix3 = new Matrix(7, 7);
            //for (var i = 0; i < 7; i++)
            //{
            //    for (var j = 0; j < 7; j++)
            //    {
            //        matrix3[i, j] = 1;
            //        matrix3[i, j] /= 1 + i + j;
            //    }
            //}
            //var vector3 = new Vector(new double[] { 1, 1, 1, 1, 1, 1, 1 });
            //vector3[0] = vector3[0] * 5699 / 420;
            //vector3[1] = vector3[1] * 4103 / 420;
            //vector3[2] = vector3[2] * 19661 / 2520;
            //vector3[3] = vector3[3] * 157 / 24;
            //vector3[4] = vector3[4] * 156631 / 27720;
            //vector3[5] = vector3[5] * 34523 / 6930;
            //vector3[6] = vector3[6] * 146077 / 32760;
            //var exactSolution3 = new Vector(new double[] { 2, 9, 4, 7, 11, 9, 2 });
            //equations.Add((matrix3, vector3, exactSolution3));

            //var matrix4 = new Matrix(2, 2);
            //matrix4[0, 0] = -41;
            //matrix4[0, 1] = 127;
            //matrix4[1, 0] = 113;
            //matrix4[1, 1] = -60;
            //var vector4 = new Vector(new double[] { -787, 1589 });
            //var exactSolution4 = new Vector(new double[] { 13, -2 });
            //equations.Add((matrix4, vector4, exactSolution4));

            //foreach (var equation in equations)
            //{
            //    var conditionNumber = new ConditionNumber(equation.Item1, equation.Item2, equation.Item3);
            //    Console.WriteLine("Left matrix is");
            //    Console.WriteLine(equation.Item1.MatrixToString());
            //    Console.WriteLine("Right matrix is");
            //    for (var i = 0; i < equation.Item1.RowCount; i++)
            //    {
            //        Console.WriteLine(equation.Item2[i]);
            //    }
            //    Console.WriteLine();
            //    Console.WriteLine("Exact solution is");
            //    for (var i = 0; i < equation.Item1.RowCount; i++)
            //    {
            //        Console.WriteLine(equation.Item3[i]);
            //    }

            //    var spectralCriterion = conditionNumber.CalculateSpectralCriterion();
            //    var volumetricCriterion = conditionNumber.CalculateVolumetricCriterion();
            //    var angleCriterion = conditionNumber.CalculateAngleCriterion();
            //    Console.WriteLine();
            //    Console.WriteLine($"Spectral criterion is {spectralCriterion}.");
            //    Console.WriteLine($"Volumetric criterion is {volumetricCriterion}.");
            //    Console.WriteLine($"Angle criterion is {angleCriterion}.");

            //    var variedSolution2 = conditionNumber.CalculateVariedSolution(0.01);
            //    var variedSolution5 = conditionNumber.CalculateVariedSolution(0.00001);
            //    var variedSolution8 = conditionNumber.CalculateVariedSolution(0.00000001);
            //    var difference2 = equation.Item3 - variedSolution2;
            //    var difference5 = equation.Item3 - variedSolution5;
            //    var difference8 = equation.Item3 - variedSolution8;
            //    double delta2 = 0;
            //    double delta5 = 0;
            //    double delta8 = 0;
            //    for (var i = 0; i < conditionNumber.size; i++)
            //    {
            //        delta2 += difference2[i] * difference2[i];
            //        delta5 += difference5[i] * difference5[i];
            //        delta8 += difference8[i] * difference8[i];
            //    }
            //    delta2 = Math.Sqrt(delta2);
            //    delta5 = Math.Sqrt(delta5);
            //    delta8 = Math.Sqrt(delta8);

            //    Console.WriteLine();
            //    Console.WriteLine($"|x - x˜| is {delta2} when variation is 0.01.");
            //    Console.WriteLine($"|x - x˜| is {delta5} when variation is 0.00001.");
            //    Console.WriteLine($"|x - x˜| is {delta8} when variation is 0.00000001.");
            //    Console.WriteLine();
            //    Console.WriteLine();
            //}
        }
    }
}
