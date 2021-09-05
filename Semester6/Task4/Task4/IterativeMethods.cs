using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Task4
{
    /// <summary>
    /// Iterative methods
    /// </summary>
    public class IterativeMethods
    {
        private Matrix<double> matrix;
        private Vector<double> vector;

        public int size { get; private set; }

        /// <summary>
        /// Iterative methods' constructor
        /// </summary>
        public IterativeMethods() { }

        /// <summary>
        /// Iterative methods' constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        public IterativeMethods(Matrix<double> matrix)
        {
            this.matrix = matrix;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Iterative methods' constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        /// <param name="vector">Given vector</param>
        public IterativeMethods(Matrix<double> matrix, Vector<double> vector)
        {
            this.matrix = matrix;
            this.vector = vector;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Solves equation with simple-iteration method
        /// </summary>
        /// <param name="xInitial">Initial x</param>
        /// <param name="epsilon">Epsilon</param>
        /// <returns>Solution and amount of iterations</returns>
        public (Vector<double>, int) SolveWithSimpleIterationMethod(Vector<double> xInitial, double epsilon)
        {
            var a = Matrix<double>.Build.Dense(size, size);
            var b = Vector<double>.Build.Dense(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    if (i != j)
                    {
                        a[i, j] = -matrix[i, j] / matrix[i, i];
                        b[i] = vector[i] / matrix[i, i];
                    }
                }
            }

            var amount = 1;
            var xCurrent = a.Multiply(xInitial) + b;
            var xPrevious = Vector<double>.Build.Dense(size);
            xInitial.CopyTo(xPrevious);
            while (amount < 500 && (xCurrent - xPrevious).L2Norm() > epsilon)
            {
                xPrevious = xCurrent;
                xCurrent = a.Multiply(xPrevious) + b;
                amount++;
            }

            return (xCurrent, amount);
        }

        /// <summary>
        /// Starts running program
        /// </summary>
        public void Start()
        {
            var equations = new List<(Matrix<double>, Vector<double>)>();

            var matrix0 =  Matrix<double>.Build.Dense(3, 3);
            matrix0[0, 0] = -400.6;
            matrix0[0, 1] = 0;
            matrix0[0, 2] = 0;
            matrix0[1, 0] = 0;
            matrix0[1, 1] = -600.4;
            matrix0[1, 2] = 0;
            matrix0[2, 0] = 0;
            matrix0[2, 1] = 0;
            matrix0[2, 2] = 200.2;
            var vector0 = Vector<double>.Build.Random(3, 42);
            equations.Add((matrix0, vector0));

            var matrix1 = Matrix<double>.Build.Dense(2, 2);
            matrix1[0, 0] = -198.1;
            matrix1[0, 1] = 202.4;
            matrix1[1, 0] = 0;
            matrix1[1, 1] = -489.2;
            var vector1 = Vector<double>.Build.Random(2, 16);
            equations.Add((matrix1, vector1));

            var matrix2 =  Matrix<double>.Build.Dense(2, 2);
            matrix2[0, 0] = -400.6;
            matrix2[0, 1] = 199.8;
            matrix2[1, 0] = 1198.8;
            matrix2[1, 1] = -600.4;
            var vector2 = Vector<double>.Build.Random(2, 42);
            equations.Add((matrix2, vector2));

            var matrix3 =  Matrix<double>.Build.Dense(2, 2);
            matrix3[0, 0] = 1;
            matrix3[0, 1] = 0.99;
            matrix3[1, 0] = 0.99;
            matrix3[1, 1] = 0.98;
            var vector3 = Vector<double>.Build.Random(2, 666);
            equations.Add((matrix3, vector3));

            var matrix4 = Matrix<double>.Build.Dense(4, 4);
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    matrix4[i, j] = 1;
                    matrix4[i, j] /= 1 + i + j;
                }
            }
            var vector4 = Vector<double>.Build.Random(4, 4);
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
            var vector5 = Vector<double>.Build.Random(5, 5);
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
            var vector6 = Vector<double>.Build.Random(6, 6);
            equations.Add((matrix6, vector6));

            foreach (var equation in equations)
            {
                if (equation.Item1.RowCount > 3)
                {
                    Console.WriteLine($"Hilbert matrix of order {equation.Item1.RowCount}.");
                }

                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", "ε", "||x - x_ε||", "Amount of iterations"));

                matrix = equation.Item1;
                vector = equation.Item2;
                size = matrix.RowCount;
                var solution = matrix.Solve(vector);

                for (var i = -2; i > -13; i -= 2)
                {
                    var epsilon = Math.Pow(10, i);
                    size = matrix.RowCount;
                    var result = SolveWithSimpleIterationMethod(vector, epsilon);
                    var newSolution = result.Item1;
                    var amount = result.Item2;
                    var error = solution.Subtract(newSolution).L2Norm();
                    Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", epsilon, error, amount));

                }
                matrix = null;
                vector = null;
                Console.WriteLine();
            }
        }
    }
}