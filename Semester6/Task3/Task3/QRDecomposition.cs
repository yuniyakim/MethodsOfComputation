using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Task3
{
    /// <summary>
    /// QR decomposition of a matrix
    /// </summary>
    public class QRDecomposition
    {
        private Matrix<double> matrix;
        private Vector<double> vector;

        public int size { get; private set; }

        /// <summary>
        /// QR decmposition's constructor
        /// </summary>
        public QRDecomposition() { }

        /// <summary>
        /// QR decmposition's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        public QRDecomposition(Matrix<double> matrix)
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
        public QRDecomposition(Matrix<double> matrix, Vector<double> vector)
        {
            this.matrix = matrix;
            this.vector = vector;
            size = matrix.RowCount;
        }

        /// <summary>
        /// Calculates Q and R matrices
        /// </summary>
        /// <returns>Q and R matrices</returns>
        public (Matrix<double>, Matrix<double>) CalculateQRMatrices()
        {
            var qMatrix = Matrix<double>.Build.DenseIdentity(size);
            var rMatrix = Matrix<double>.Build.Dense(size, size);
            matrix.CopyTo(rMatrix);
            for (var i = 0; i < size; i++)
            {
                for (var j = i + 1; j < size; j++)
                {
                    var c = rMatrix[i, i] / Math.Sqrt(Math.Pow(rMatrix[i, i], 2) + Math.Pow(rMatrix[j, i], 2));
                    var s = rMatrix[j, i] / Math.Sqrt(Math.Pow(rMatrix[i, i], 2) + Math.Pow(rMatrix[j, i], 2));
                    var rMatrixCopy = Matrix<double>.Build.Dense(size, size); 
                    rMatrix.CopyTo(rMatrixCopy);
                    var qMatrixCopy = Matrix<double>.Build.Dense(size, size); 
                    qMatrix.CopyTo(qMatrixCopy);
                    for (var k = 0; k < size; k++)
                    {
                        rMatrix[i, k] = c * rMatrixCopy[i, k] + s * rMatrixCopy[j, k];
                        rMatrix[j, k] = (-s) * rMatrixCopy[i, k] + c * rMatrixCopy[j, k];
                        qMatrix[k, i] = c * qMatrixCopy[k, i] + s * qMatrixCopy[k, j];
                        qMatrix[k, j] = (-s) * qMatrixCopy[k, i] + c * qMatrixCopy[k, j];
                    }
                }
            }
            return (qMatrix, rMatrix);
        }

        /// <summary>
        /// Solves linear equation with QR-decomposition
        /// </summary>
        /// <returns>Solution of equation</returns>
        public Vector<double> SolveEquationWithQRDecomposition()
        {
            var qrMatrices = CalculateQRMatrices();
            var qMatrix = qrMatrices.Item1;
            var rMatrix = qrMatrices.Item2;

            var solution = Vector<double>.Build.Dense(size);
            var qTb = qMatrix.TransposeThisAndMultiply(vector);
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < i; j++)
                {
                    sum += rMatrix[size - i - 1, size - j - 1] * solution[size - j - 1];
                }
                solution[size - i - 1] = (qTb[size - i - 1] - sum) / rMatrix[size - i - 1, size - i - 1];
            }
            return solution;
        }

        /// <summary>
        /// Starts running program
        /// </summary>
        public void Start()
        {
            var A = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                    {  12,  -51,    4 },
                    {   6,  167,  -68 },
                    {  -4,   24,  -41 }
            });
            Console.WriteLine("A:");
            Console.WriteLine(A);
            var qr = A.QR();
            Console.WriteLine();
            Console.WriteLine("Q:");
            Console.WriteLine(qr.Q);
            Console.WriteLine();

            matrix = A;
            size = A.RowCount;
            Console.WriteLine(CalculateQRMatrices().Item1);
            Console.WriteLine();
            Console.WriteLine(CalculateQRMatrices().Item2);

            Console.WriteLine();
            //    var equations = new List<(Matrix<double>, Vector<double>)>();

            //    var matrix4 = Matrix<double>.Build.Dense(4, 4);
            //    for (var i = 0; i < 4; i++)
            //    {
            //        for (var j = 0; j < 4; j++)
            //        {
            //            matrix4[i, j] = 1;
            //            matrix4[i, j] /= 1 + i + j;
            //        }
            //    }
            //    var vector4 = Vector<double>.Build.Dense(4, 1.0);
            //    equations.Add((matrix4, vector4));

            //    var matrix5 = Matrix<double>.Build.Dense(5, 5);
            //    for (var i = 0; i < 5; i++)
            //    {
            //        for (var j = 0; j < 5; j++)
            //        {
            //            matrix5[i, j] = 1;
            //            matrix5[i, j] /= 1 + i + j;
            //        }
            //    }
            //    var vector5 = Vector<double>.Build.Dense(5, 1);
            //    equations.Add((matrix5, vector5));

            //    var matrix6 = Matrix<double>.Build.Dense(6, 6);
            //    for (var i = 0; i < 6; i++)
            //    {
            //        for (var j = 0; j < 6; j++)
            //        {
            //            matrix6[i, j] = 1;
            //            matrix6[i, j] /= 1 + i + j;
            //        }
            //    }
            //    var vector6 = Vector<double>.Build.Dense(6, 1);
            //    equations.Add((matrix6, vector6));

            //    var matrix7 = Matrix<double>.Build.Dense(7, 7);
            //    for (var i = 0; i < 7; i++)
            //    {
            //        for (var j = 0; j < 7; j++)
            //        {
            //            matrix7[i, j] = 1;
            //            matrix7[i, j] /= 1 + i + j;
            //        }
            //    }
            //    var vector7 = Vector<double>.Build.Dense(7, 1);
            //    equations.Add((matrix7, vector7));

            //    foreach (var equation in equations)
            //    {
            //        Console.WriteLine($"Hilbert matrix of order {equation.Item1.RowCount}.");

            //        Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", "α", "cond (A + αE)", "||x - x_α||", "||b - Ax_α||"));

            //        matrix = equation.Item1;
            //        vector = equation.Item2;
            //        var solution = matrix.Solve(vector);
            //        Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", 0, matrix.ConditionNumber(), 0, 0));

            //        for (var i = -1; i > -13; i--)
            //        {
            //            var alpha = Math.Pow(10, i);
            //            matrix = equation.Item1.Add(alpha);
            //            var cond = matrix.ConditionNumber();
            //            size = matrix.RowCount;
            //            var newSolution = SolveEquationWithQRDecomposition();
            //            var error = solution.Subtract(newSolution).L2Norm();
            //            var norm = vector.Subtract(equation.Item1.Multiply(newSolution)).L2Norm();
            //            Console.WriteLine(string.Format("|{0,-10}|{1,-25}|{2,-25}|{3,25}|", alpha, cond, error, norm));
            //        }
            //        matrix = null;
            //        vector = null;
            //        Console.WriteLine();
            //    }
        }
    }
}