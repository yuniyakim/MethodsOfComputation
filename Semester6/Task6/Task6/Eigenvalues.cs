using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Task6
{
    /// <summary>
    /// Eigenvalue
    /// </summary>
    public class Eigenvalues
    {
        private Matrix<double> matrix;
        private Vector<double> vector;

        public int size { get; private set; }

        /// <summary>
        /// Eigenvalues' constructor
        /// </summary>
        public Eigenvalues() { }

        /// <summary>
        /// Eigenvalues' constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        public Eigenvalues(Matrix<double> matrix)
        {
            this.matrix = matrix;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Eigenvalues' constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        /// <param name="vector">Given vector</param>
        public Eigenvalues(Matrix<double> matrix, Vector<double> vector)
        {
            this.matrix = matrix;
            this.vector = vector;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Finds max module element in matrix
        /// </summary>
        /// <param name="currentMatrix">Matrix to analyze</param>
        /// <returns>Row and column numbers</returns>
        public (int, int) FindMaxModuleElementInMatrix(Matrix<double> currentMatrix)
        {
            var iMax = 0;
            var jMax = 1;
            var maxModuleElement = currentMatrix[iMax, jMax];
            for (var i = 0; i < currentMatrix.RowCount; i++)
            {
                for (var j = i + 1; j < currentMatrix.RowCount; j++)
                {
                    if (Math.Abs(maxModuleElement) < Math.Abs(currentMatrix[i, j]))
                    {
                        maxModuleElement = currentMatrix[i, j];
                        iMax = i;
                        jMax = j;
                    }
                }
            }
            return (iMax, jMax);
        }

        /// <summary>
        /// Calculates eigenvalues with Jacobi method
        /// </summary>
        /// <param name="epsilon">Epsilon</param>
        /// <param name="isStrategyMaxModule">Indicator if chosen strategy is max module element</param>
        /// <returns></returns>
        public (Vector<double>, int) CalculateEigenvaluesWithJacobiMethod(double epsilon, bool isStrategyMaxModule)
        {
            var amount = 0;
            var i = 0;
            var j = 0;
            var currentMatrix = matrix.Clone();
            while (true)
            {
                var hMatrix = Matrix<double>.Build.DenseIdentity(currentMatrix.RowCount);
                if (isStrategyMaxModule)
                {
                    var element = FindMaxModuleElementInMatrix(currentMatrix);
                    i = element.Item1;
                    j = element.Item2;
                }
                else
                {
                    if (j < currentMatrix.RowCount - 1 && j + 1 != i)
                    {
                        j++;
                    }
                    else if (j == currentMatrix.RowCount - 1)
                    {
                        i++;
                        j = 0;
                    }
                    else
                    {
                        j += 2;
                    }
                }
                if ((i == currentMatrix.RowCount - 1 && j == currentMatrix.RowCount) || Math.Abs(currentMatrix[i, j]) < epsilon)
                {
                    return (currentMatrix.Diagonal(), amount);
                }
                amount++;
                var phi = 1 / 2 * Math.Atan(2 * currentMatrix[i, j] / (currentMatrix[i, i] - currentMatrix[j, j]));
                hMatrix[i, i] = Math.Cos(phi);
                hMatrix[j, j] = Math.Cos(phi);
                hMatrix[i, j] = -Math.Sin(phi);
                hMatrix[j, i] = Math.Sin(phi);
                currentMatrix = hMatrix.TransposeThisAndMultiply(currentMatrix).Multiply(hMatrix);
            }
        }

        /// <summary>
        /// Starts running program
        /// </summary>
        public void Start()
        {
            var equations = new List<(Matrix<double>, Vector<double>)>();

            var matrix1 = Matrix<double>.Build.Dense(3, 3);
            matrix1[0, 0] = -198.1;
            matrix1[0, 1] = 389.9;
            matrix1[0, 2] = 123.2;
            matrix1[1, 0] = 0;
            matrix1[1, 1] = 202.4;
            matrix1[1, 2] = 249.3;
            matrix1[2, 0] = 0;
            matrix1[2, 1] = 0;
            matrix1[2, 2] = -489.2;
            var vector1 = Vector<double>.Build.Random(3, 16);
            equations.Add((matrix1, vector1));

            var matrix2 = Matrix<double>.Build.Dense(5, 5);
            matrix2[0, 0] = 2;
            matrix2[0, 1] = -1;
            matrix2[0, 2] = 0;
            matrix2[0, 3] = 0;
            matrix2[0, 4] = 0;
            matrix2[1, 0] = -3;
            matrix2[1, 1] = 8;
            matrix2[1, 2] = -1;
            matrix2[1, 3] = 0;
            matrix2[1, 4] = 0;
            matrix2[2, 0] = 0;
            matrix2[2, 1] = -5;
            matrix2[2, 2] = 12;
            matrix2[2, 3] = 2;
            matrix2[2, 4] = 0;
            matrix2[3, 0] = 0;
            matrix2[3, 1] = 0;
            matrix2[3, 2] = -6;
            matrix2[3, 3] = 18;
            matrix2[3, 4] = -4;
            matrix2[4, 0] = 0;
            matrix2[4, 1] = 0;
            matrix2[4, 2] = 0;
            matrix2[4, 3] = -5;
            matrix2[4, 4] = 10;
            var vector2 = Vector<double>.Build.Random(5, 10);
            equations.Add((matrix2, vector2));

            var matrix3 = Matrix<double>.Build.Dense(2, 2);
            matrix3[0, 0] = 1;
            matrix3[0, 1] = 0.99;
            matrix3[1, 0] = 0.99;
            matrix3[1, 1] = 0.98;
            var vector3 = Vector<double>.Build.Random(2, 666);
            equations.Add((matrix3, vector3));

            var matrix31 = Matrix<double>.Build.Dense(2, 2);
            matrix31[0, 0] = -401.98;
            matrix31[0, 1] = 200.34;
            matrix31[1, 0] = 1202.04;
            matrix31[1, 1] = -602.32;
            var vector31 = Vector<double>.Build.Random(2, 666);
            equations.Add((matrix31, vector31));

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
                Console.WriteLine("Power method");
                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", "ε", "|lambda - lambda_ε|", "Amount of iterations"));

                matrix = equation.Item1;
                vector = equation.Item2;
                size = matrix.RowCount;
                var eigenvalue = matrix.Evd().EigenValues[size - 1].Real;

                for (var i = -2; i > -6; i--)
                {
                    var epsilon = Math.Pow(10, i);
                    var result = CalculateEigenvaluesWithJacobiMethod(vector, epsilon);
                    var newEigenvalue = result.Item1;
                    var amount = result.Item2;
                    var error = Math.Abs(eigenvalue - newEigenvalue);
                    Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", epsilon, error, amount));
                }

                Console.WriteLine();
                Console.WriteLine("Scalar products method");
                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", "ε", "|lambda - lambda_ε|", "Amount of iterations"));

                for (var i = -2; i > -6; i--)
                {
                    var epsilon = Math.Pow(10, i);
                    size = matrix.RowCount;
                    var result = CalculateMaxEigenvalueWithScalarProductsMethod(vector, epsilon);
                    var newEigenvalue = result.Item1;
                    var amount = result.Item2;
                    var error = Math.Abs(eigenvalue - newEigenvalue);
                    Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", epsilon, error, amount));
                }

                matrix = null;
                vector = null;
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}