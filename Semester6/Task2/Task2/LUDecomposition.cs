using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Task2
{
    /// <summary>
    /// LU decomposition of a matrix
    /// </summary>
    public class LUDecomposition
    {
        private Matrix<double>  matrix;
        private Vector<double> vector;

        public int size { get; private set; }

        /// <summary>
        /// LU decmposition's constructor
        /// </summary>
        public LUDecomposition() { }

        /// <summary>
        /// LU decmposition's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        public LUDecomposition(Matrix<double> matrix)
        {
            this.matrix = matrix;
            size = matrix.RowCount;
        }

        /// <summary>
        /// LU decmposition's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        /// <param name="vector">Given vector</param>
        /// <param name="exactSolution">Given exact solution</param>
        public LUDecomposition(Matrix<double> matrix, Vector<double> vector)
        {
            this.matrix = matrix;
            this.vector = vector;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Calculates L and U matrices
        /// </summary>
        /// <returns>L and U matrices</returns>
        public (Matrix<double>, Matrix<double>) CalculateLUMatrices()
        {
            var lMatrix = Matrix<double>.Build.Dense(size, size);
            var uMatrix = Matrix<double>.Build.Dense(size, size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (i <= j)
                    {
                        lMatrix[i, j] = (i == j) ? 1 : 0;
                        double sum = 0;
                        for (var k = 0; k < i; k++)
                        {
                            sum += lMatrix[i, k] * uMatrix[k, j];
                        }
                        uMatrix[i, j] = matrix[i, j] - sum;
                    }
                    else
                    {
                        uMatrix[i, j] = 0;
                        double sum = 0;
                        for (var k = 0; k < j; k++)
                        {
                            sum += lMatrix[i, k] * uMatrix[k, j];
                        }
                        lMatrix[i, j] = (matrix[i, j] - sum) / uMatrix[j, j];
                    }
                }
            }
            return (lMatrix, uMatrix);
        }

        /// <summary>
        /// Solves linear equation with LU decomposition
        /// </summary>
        /// <returns>Solution of equation</returns>
        public Vector<double> SolveEquationWithLUDecomposition()
        {
            var luMatrices = CalculateLUMatrices();
            var lMatrix = luMatrices.Item1;
            var uMatrix = luMatrices.Item2;

            var ySolution = Vector<double>.Build.Dense(size);
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < i; j++)
                {
                    sum += lMatrix[i, j] * ySolution[j];
                }
                ySolution[i] = vector[i] - sum;
            }

            var xSolution = Vector<double>.Build.Dense(size);
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < i; j++)
                {
                    sum += uMatrix[size - i - 1, size - j - 1] * xSolution[size - j - 1];
                }
                xSolution[size - i - 1] = (ySolution[size - i - 1] - sum) / uMatrix[size - i - 1, size - i - 1];
            }

            return xSolution;
        }

        /// <summary>
        /// Starts running program
        /// </summary>
        public void Start()
        {
            var equations = new List<(Matrix<double>, Vector<double>)>();

            var matrix4 = Matrix<double>.Build.Dense(4, 4);
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix4[i, j] = 1;
                    matrix4[i, j] /= 1 + i + j;
                }
            }
            var vector4 = Vector<double>.Build.Dense(4, 1.0);
            equations.Add((matrix4, vector4));

            var matrix5 = Matrix<double>.Build.Dense(5, 5);
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    matrix5[i, j] = 1;
                    matrix5[i, j] /= 1 + i + j;
                }
            }
            var vector5 = Vector<double>.Build.Dense(5, 1);
            equations.Add((matrix5, vector5));

            var matrix6 = Matrix<double>.Build.Dense(6, 6);
            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    matrix6[i, j] = 1;
                    matrix6[i, j] /= 1 + i + j;
                }
            }
            var vector6 = Vector<double>.Build.Dense(6, 1);
            equations.Add((matrix6, vector6));

            var matrix7 = Matrix<double>.Build.Dense(7, 7);
            for (var i = 0; i < 7; i++)
            {
                for (var j = 0; j < 7; j++)
                {
                    matrix7[i, j] = 1;
                    matrix7[i, j] /= 1 + i + j;
                }
            }
            var vector7 = Vector<double>.Build.Dense(7, 1);
            equations.Add((matrix7, vector7));

            foreach (var equation in equations)
            {
                Console.WriteLine($"Hilbert matrix of order {equation.Item1.RowCount}.");

                Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", "α", "cond (A + αE)", "||x - x_α||", "||b - Ax_α||"));

                matrix = equation.Item1;
                vector = equation.Item2;
                var solution = matrix.Solve(vector);
                Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", 0, matrix.ConditionNumber(), 0, 0));

                for (var i = -1; i > -13; i--)
                {
                    var alpha = Math.Pow(10, i);
                    matrix = equation.Item1.Add(alpha);
                    var cond = matrix.ConditionNumber();
                    size = matrix.RowCount;
                    var newSolution = SolveEquationWithLUDecomposition();
                    var error = solution.Subtract(newSolution).L2Norm();
                    var norm = vector.Subtract(equation.Item1.Multiply(newSolution)).L2Norm();
                    Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", alpha, cond, error, norm));
                }
                matrix = null;
                vector = null;
                Console.WriteLine();
            }
        }
    }
}
