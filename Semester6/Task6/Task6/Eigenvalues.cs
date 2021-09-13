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
        /// Defines if eigenvalue belongs to Gershgorin circles
        /// </summary>
        /// <param name="aMatrix">Matrix</param>
        /// <param name="eigenvalue">Eigenvalue</param>
        /// <returns>True if eigenvalue belongs to Gershgorin circles, false otherwise</returns>
        public bool IsInGershgorinCircle(Matrix<double> aMatrix, double eigenvalue)
        {
            var circles = new List<(double, double)>();
            for (var i = 0; i < aMatrix.RowCount; i++)
            {
                double sum = 0;
                for (var j = 0; j < aMatrix.ColumnCount; j++)
                {
                    sum += Math.Abs(aMatrix[i, j]);
                }
                circles.Add((aMatrix[i, i], sum - Math.Abs(aMatrix[i, i])));
            }

            foreach (var circle in circles)
            {
                if (Math.Abs(circle.Item1 - eigenvalue) <= circle.Item2)
                {
                    return true;
                }
            }
            return false;
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
                if (Math.Abs(currentMatrix[i, j]) < epsilon)
                {
                    return (currentMatrix.Diagonal(), amount);
                }
                amount++;
                var phi = Math.Atan(2 * currentMatrix[i, j] / (currentMatrix[i, i] - currentMatrix[j, j])) / 2;
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

            var matrix2 = Matrix<double>.Build.Dense(3, 3);
            matrix2[0, 0] = -0.81417;
            matrix2[0, 1] = -0.01937;
            matrix2[0, 2] = 0.41372;
            matrix2[1, 0] = -0.01937;
            matrix2[1, 1] = 0.54414;
            matrix2[1, 2] = 0.00590;
            matrix2[2, 0] = 0.41372;
            matrix2[2, 1] = 0.00590;
            matrix2[2, 2] = -0.81445;
            var vector2 = Vector<double>.Build.Random(3, 10);
            equations.Add((matrix2, vector2));

            var matrix3 = Matrix<double>.Build.Dense(3, 3);
            matrix3[0, 0] = -1.51898;
            matrix3[0, 1] = -0.19907;
            matrix3[0, 2] = 0.95855;
            matrix3[1, 0] = -0.19907;
            matrix3[1, 1] = 1.17742;
            matrix3[1, 2] = 0.06992;
            matrix3[2, 0] = 0.95855;
            matrix3[2, 1] = 0.06992;
            matrix3[2, 2] = -1.57151;
            var vector3 = Vector<double>.Build.Random(3, 666);
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

            var matrix6 = Matrix<double>.Build.Dense(15, 15);
            for (var i = 0; i < 15; i++)
            {
                for (var j = 0; j < 15; j++)
                {
                    matrix6[i, j] = 1;
                    matrix6[i, j] /= 1 + i + j;
                }
            }
            var vector6 = Vector<double>.Build.Random(15, 15);
            equations.Add((matrix6, vector6));

            var matrix7 = Matrix<double>.Build.Dense(10, 10);
            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    matrix7[i, j] = 1;
                    matrix7[i, j] /= 1 + i + j;
                }
            }
            var vector7 = Vector<double>.Build.Random(10, 10);
            equations.Add((matrix7, vector7));

            foreach (var equation in equations)
            {
                Console.WriteLine("Max module eigenvalue strategy");
                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", "ε", "|lambda - lambda_ε|", "Amount of iterations"));

                matrix = equation.Item1;
                vector = equation.Item2;
                size = matrix.RowCount;
                var eigenvalue = matrix.Evd().EigenValues[size - 1].Real;

                for (var i = -2; i > -6; i--)
                {
                    var epsilon = Math.Pow(10, i);
                    var result = CalculateEigenvaluesWithJacobiMethod(epsilon, true);
                    var newEigenvalue = result.Item1;
                    var amount = result.Item2;
                    var error = (newEigenvalue - eigenvalue).L2Norm();
                    Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", epsilon, error, amount));
                }

                Console.WriteLine();
                Console.WriteLine("Zeroing in order strategy");
                Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", "ε", "|lambda - lambda_ε|", "Amount of iterations"));

                for (var i = -2; i > -6; i--)
                {
                    var epsilon = Math.Pow(10, i);
                    size = matrix.RowCount;
                    var result = CalculateEigenvaluesWithJacobiMethod(epsilon, false);
                    var newEigenvalue = result.Item1;
                    var amount = result.Item2;
                    var error = (newEigenvalue - eigenvalue).L2Norm();
                    Console.WriteLine(string.Format("{0,-10}|{1,-25}|{2,-25}", epsilon, error, amount));
                }

                Console.WriteLine();
                var eigenvalues = CalculateEigenvaluesWithJacobiMethod(Math.Pow(10, -5), true).Item1;
                var flag = true;
                foreach (var value in eigenvalues)
                {
                    if (!IsInGershgorinCircle(matrix, value))
                    {
                        flag = false;
                    }
                }
                Console.WriteLine($"Eigenvalue belongs to Gershgorin circle: {flag}.");

                matrix = null;
                vector = null;
                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}