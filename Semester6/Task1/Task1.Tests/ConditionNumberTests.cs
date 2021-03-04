using NUnit.Framework;
using System;
using System.Collections.Generic;
using DotNumerics.LinearAlgebra;

namespace Task1.Tests
{
    public class Tests
    {
        private ConditionNumber conditionNumber;

        [SetUp]
        public void Setup()
        {
            conditionNumber = null;
        }

        [Test]
        public void VolumetricAndAngleCriteriaDiagonalMatrixTest()
        {
            var matrix = new Matrix(3, 3);
            matrix[0, 0] = -400.6;
            matrix[0, 1] = 0;
            matrix[0, 2] = 0;
            matrix[1, 0] = 0;
            matrix[1, 1] = -600.4;
            matrix[1, 2] = 0;
            matrix[2, 0] = 0;
            matrix[2, 1] = 0;
            matrix[2, 2] = 200.2;
            conditionNumber = new ConditionNumber(matrix, null, null);
            Assert.AreEqual(1, conditionNumber.CalculateVolumetricCriterion());
            Assert.AreEqual(1, conditionNumber.CalculateAngleCriterion());
        }

        [Test]
        public void VolumetricAndAngleCriteriaEqualityTest()
        {
            var matrix = new Matrix(2, 2);
            matrix[0, 0] = 4;
            matrix[0, 1] = 3;
            matrix[1, 0] = 9;
            matrix[1, 1] = 2;
            conditionNumber = new ConditionNumber(matrix, null, null);
            Assert.AreEqual(conditionNumber.CalculateVolumetricCriterion(), conditionNumber.CalculateAngleCriterion());
        }

        [Test]
        public void VariedMatrixAndVectorCalculationTest()
        {
            var equations = new List<(Matrix, Vector, Vector)>();

            var matrix1 = new Matrix(2, 2);
            matrix1[0, 0] = -400.6;
            matrix1[0, 1] = 199.8;
            matrix1[1, 0] = 1198.8;
            matrix1[1, 1] = -600.4;
            var vector1 = new Vector(new double[] { 200, -600 });
            var exactSolution1 = new Vector(new double[] { -0.2, 0.6 });
            equations.Add((matrix1, vector1, exactSolution1));

            var matrix2 = new Matrix(2, 2);
            matrix2[0, 0] = -200.4;
            matrix2[0, 1] = -398.6;
            matrix2[1, 0] = 1060.8;
            matrix2[1, 1] = 400.2;
            var vector2 = new Vector(new double[] { -40.74, 728.58 });
            var exactSolution2 = new Vector(new double[] { 0.8, -0.3 });
            equations.Add((matrix2, vector2, exactSolution2));

            var matrix3 = new Matrix(2, 2);
            matrix3[0, 0] = 58.4;
            matrix3[0, 1] = -60;
            matrix3[1, 0] = 14.2;
            matrix3[1, 1] = 34.6;
            var vector3 = new Vector(new double[] { 47.68, -17.92 });
            var exactSolution3 = new Vector(new double[] { 0.2, -0.6 });
            equations.Add((matrix3, vector3, exactSolution3));

            foreach (var equation in equations)
            {
                var conditionNumber = new ConditionNumber(equation.Item1, equation.Item2, equation.Item3);
                var variedSolution1 = conditionNumber.CalculateVariedSolution(0.01);
                var variedSolution2 = conditionNumber.CalculateVariedSolution(0.2);
                var variedSolution3 = conditionNumber.CalculateVariedSolution(3);

                var variedMatrix1 = equation.Item1.Clone();
                var variedMatrix2 = equation.Item1.Clone();
                var variedMatrix3 = equation.Item1.Clone();
                var variedVector1 = equation.Item2.Clone();
                var variedVector2 = equation.Item2.Clone();
                var variedVector3 = equation.Item2.Clone();
                for (var i = 0; i < conditionNumber.size; i++)
                {
                    for (var j = 0; j < conditionNumber.size; j++)
                    {
                        variedMatrix1[i, j] += 0.01;
                        variedMatrix2[i, j] += 0.2;
                        variedMatrix3[i, j] += 3;
                    }
                    variedVector1[i] += 0.01;
                    variedVector2[i] += 0.2;
                    variedVector3[i] += 3;
                }

                var actualVector1 = variedMatrix1 * variedSolution1;
                var actualVector2 = variedMatrix2 * variedSolution2;
                var actualVector3 = variedMatrix3 * variedSolution3;
                for (var i = 0; i < conditionNumber.size; i++)
                {
                    Assert.That(variedVector1[i], Is.EqualTo(actualVector1[i, 0]).Within(Math.Pow(10, -10)));
                    Assert.That(variedVector2[i], Is.EqualTo(actualVector2[i, 0]).Within(Math.Pow(10, -10)));
                    Assert.That(variedVector3[i], Is.EqualTo(actualVector3[i, 0]).Within(Math.Pow(10, -10)));
                }
            }
        }

        [Test]
        public void CriteriaBadMatrixTest()
        {
            var matrix = new Matrix(2, 2);
            matrix[0, 0] = 1;
            matrix[0, 1] = 0.99;
            matrix[1, 0] = 0.99;
            matrix[1, 1] = 0.98;
            conditionNumber = new ConditionNumber(matrix, null, null);
            Assert.IsTrue(conditionNumber.CalculateSpectralCriterion() > 10000);
            Assert.IsTrue(conditionNumber.CalculateVolumetricCriterion() > 10000);
            Assert.IsTrue(conditionNumber.CalculateAngleCriterion() > 10000);
        }

        [Test]
        public void CriteriaGilbertMatrixTest()
        {
            var matrix = new Matrix(6, 6);
            for (var i = 0; i < 6; i++)
            {
                for (var j = 0; j < 6; j++)
                {
                    matrix[i, j] = 1;
                    matrix[i, j] /= 1 + i + j;
                }
            }
            conditionNumber = new ConditionNumber(matrix, null, null);
            Assert.IsTrue(conditionNumber.CalculateSpectralCriterion() > 10000);
            Assert.IsTrue(conditionNumber.CalculateVolumetricCriterion() > 10000);
            Assert.IsTrue(conditionNumber.CalculateAngleCriterion() > 10000);
        }
    }
}