using NUnit.Framework;
using System;
using MathNet.Numerics.LinearAlgebra;

namespace Task3.Tests
{
    public class Tests
    {
        private QRDecomposition qrDecomposition;

        [SetUp]
        public void Setup()
        {
            qrDecomposition = null;
        }

        [Test]
        public void CalculateQRMatricesTest()
        {
            var matrix = Matrix<double>.Build.Dense(2, 2);
            var size = matrix.RowCount;
            matrix[0, 0] = -400.6;
            matrix[0, 1] = 199.8;
            matrix[1, 0] = 1198.8;
            matrix[1, 1] = -600.4;

            qrDecomposition = new QRDecomposition(matrix);
            var luMatrices = qrDecomposition.CalculateQRMatrices();
            var lMatrix = luMatrices.Item1;
            var uMatrix = luMatrices.Item2;
            var luMatrix = lMatrix * uMatrix;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    Assert.That(luMatrix[i, j], Is.EqualTo(matrix[i, j]).Within(Math.Pow(10, -10)));
                }
            }
        }

        [Test]
        public void SolveEquationTest()
        {
            var matrix = Matrix<double>.Build.Dense(2, 2);
            var size = matrix.RowCount;
            matrix[0, 0] = -400.6;
            matrix[0, 1] = 199.8;
            matrix[1, 0] = 1198.8;
            matrix[1, 1] = -600.4;
            var vector = Vector<double>.Build.DenseOfArray(new double[] { 200, -600 });
            var exactSolution = Vector<double>.Build.DenseOfArray(new double[] { -0.2, 0.6 });

            qrDecomposition = new QRDecomposition(matrix, vector);
            var solution = qrDecomposition.SolveEquationWithQRDecomposition();
            for (var i = 0; i < size; i++)
            {
                Assert.That(solution[i], Is.EqualTo(exactSolution[i]).Within(Math.Pow(10, -10)));
            }
        }

        [Test]
        public void SolveEquationWithBadMatrixTest()
        {
            var matrix = Matrix<double>.Build.Dense(2, 2);
            var size = matrix.RowCount;
            matrix[0, 0] = 1;
            matrix[0, 1] = 0.99;
            matrix[1, 0] = 0.99;
            matrix[1, 1] = 0.98;
            var vector = Vector<double>.Build.DenseOfArray(new double[] { -0.393, -0.389 });
            var exactSolution = Vector<double>.Build.DenseOfArray(new double[] { 0.3, -0.7 });

            qrDecomposition = new QRDecomposition(matrix, vector);
            var solution = qrDecomposition.SolveEquationWithQRDecomposition();
            for (var i = 0; i < size; i++)
            {
                Assert.That(solution[i], Is.EqualTo(exactSolution[i]).Within(Math.Pow(10, -10)));
            }
        }

        [Test]
        public void SolveEquationWithDiagonalMatrixTest()
        {
            var matrix = Matrix<double>.Build.Dense(3, 3);
            var size = matrix.RowCount;
            matrix[0, 0] = -400.6;
            matrix[0, 1] = 0;
            matrix[0, 2] = 0;
            matrix[1, 0] = 0;
            matrix[1, 1] = -600.4;
            matrix[1, 2] = 0;
            matrix[2, 0] = 0;
            matrix[2, 1] = 0;
            matrix[2, 2] = 200.2;
            var vector = Vector<double>.Build.DenseOfArray(new double[] { -200.3, 120.08, 80.08 });
            var exactSolution = Vector<double>.Build.DenseOfArray(new double[] { 0.5, -0.2, 0.4 });

            qrDecomposition = new QRDecomposition(matrix, vector);
            var solution = qrDecomposition.SolveEquationWithQRDecomposition();
            for (var i = 0; i < size; i++)
            {
                Assert.That(solution[i], Is.EqualTo(exactSolution[i]).Within(Math.Pow(10, -10)));
            }
        }

        [Test]
        public void SolveEquationWithHilbertMatrixTest()
        {
            var matrix = Matrix<double>.Build.Dense(7, 7);
            var size = matrix.RowCount;
            for (var i = 0; i < 7; i++)
            {
                for (var j = 0; j < 7; j++)
                {
                    matrix[i, j] = 1;
                    matrix[i, j] /= 1 + i + j;
                }
            }
            var vector = Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1, 1, 1, 1 });
            vector[0] = vector[0] * 5699 / 420;
            vector[1] = vector[1] * 4103 / 420;
            vector[2] = vector[2] * 19661 / 2520;
            vector[3] = vector[3] * 157 / 24;
            vector[4] = vector[4] * 156631 / 27720;
            vector[5] = vector[5] * 34523 / 6930;
            vector[6] = vector[6] * 146077 / 32760;
            var exactSolution = Vector<double>.Build.DenseOfArray(new double[] { 2, 9, 4, 7, 11, 9, 2 });

            qrDecomposition = new QRDecomposition(matrix, vector);
            var solution = qrDecomposition.SolveEquationWithQRDecomposition();
            for (var i = 0; i < size; i++)
            {
                Assert.That(solution[i], Is.EqualTo(exactSolution[i]).Within(Math.Pow(10, -7)));
            }
        }
    }
}