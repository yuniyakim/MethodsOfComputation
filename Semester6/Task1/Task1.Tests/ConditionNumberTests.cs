using NUnit.Framework;
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
    }
}