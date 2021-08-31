using DotNumerics.LinearAlgebra;
using NUnit.Framework;
using System;

namespace Task2.Tests
{
    public class Tests
    {
        private LUDecomposition luDecomposition;

        [SetUp]
        public void Setup()
        {
            luDecomposition = null;
        }

        [Test]
        public void SimpleCalculateLUMatricesTest()
        {
            var matrix = new Matrix(3);
            var size = matrix.RowCount;
            matrix[0, 0] = 1;
            matrix[0, 1] = 2;
            matrix[0, 2] = 4;
            matrix[1, 0] = 3;
            matrix[1, 1] = 8;
            matrix[1, 2] = 14;
            matrix[2, 0] = 2;
            matrix[2, 1] = 6;
            matrix[2, 2] = 13;

            var lMatrixCorrect = new Matrix(3);
            lMatrixCorrect[0, 0] = 1;
            lMatrixCorrect[0, 1] = 0;
            lMatrixCorrect[0, 2] = 0;
            lMatrixCorrect[1, 0] = 3;
            lMatrixCorrect[1, 1] = 1;
            lMatrixCorrect[1, 2] = 0;
            lMatrixCorrect[2, 0] = 2;
            lMatrixCorrect[2, 1] = 1;
            lMatrixCorrect[2, 2] = 1;

            var uMatrixCorrect = new Matrix(3);
            uMatrixCorrect[0, 0] = 1;
            uMatrixCorrect[0, 1] = 2;
            uMatrixCorrect[0, 2] = 4;
            uMatrixCorrect[1, 0] = 0;
            uMatrixCorrect[1, 1] = 2;
            uMatrixCorrect[1, 2] = 2;
            uMatrixCorrect[2, 0] = 0;
            uMatrixCorrect[2, 1] = 0;
            uMatrixCorrect[2, 2] = 3;

            luDecomposition = new LUDecomposition(matrix);
            var luMatrices = luDecomposition.CalculateLUMatrices();
            var lMatrix = luMatrices.Item1;
            var uMatrix = luMatrices.Item2;
            var luMatrix = lMatrix * uMatrix;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    Assert.AreEqual(lMatrixCorrect[i, j], lMatrix[i, j]);
                    Assert.AreEqual(uMatrixCorrect[i, j], uMatrix[i, j]);
                    Assert.AreEqual(matrix[i, j], luMatrix[i, j]);
                }
            }
        }

        [Test]
        public void CalculateLUMatricesTest()
        {
            var matrix = new Matrix(2);
            var size = matrix.RowCount;
            matrix[0, 0] = -400.6;
            matrix[0, 1] = 199.8;
            matrix[1, 0] = 1198.8;
            matrix[1, 1] = -600.4;

            luDecomposition = new LUDecomposition(matrix);
            var luMatrices = luDecomposition.CalculateLUMatrices();
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
    }
}