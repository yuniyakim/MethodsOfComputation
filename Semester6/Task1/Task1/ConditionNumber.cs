using System;
using System.Collections.Generic;
using DotNumerics.LinearAlgebra;
using Nomad.Core;
using Nomad.Utility;

namespace Task1
{
    /// <summary>
    /// Condition number of a matrix
    /// </summary>
    public class ConditionNumber
    {
        private DotNumerics.LinearAlgebra.Matrix leftMatrix;
        private DotNumerics.LinearAlgebra.Matrix rightMatrix;
        private int size;

        //public double SpectralCriterion { get; private set; }
        //public double VolumetricCriterion { get; private set; }
        //public double AngleCriterion { get; private set; }

        /// <summary>
        /// Condition number's constructor
        /// </summary>
        /// <param name="leftMatrix"></param>
        /// <param name="rightMatrix"></param>
        public ConditionNumber(DotNumerics.LinearAlgebra.Matrix leftMatrix, DotNumerics.LinearAlgebra.Matrix rightMatrix)
        {
            this.leftMatrix = leftMatrix;
            this.rightMatrix = rightMatrix;
            size = leftMatrix.RowCount;
        }

        /// <summary>
        /// Calculates spectral criterion of the left matrix
        /// </summary>
        public double CalculateSpectralCriterion()
        {
            var matrix = new Nomad.Core.Matrix(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    matrix[i, j] = leftMatrix[i, j];
                }
            }

            var norm = matrix.EuclideanNorm();
            var normInverse = matrix.Inverse().EuclideanNorm();
            return norm * normInverse;
        }

        /// <summary>
        /// Calculates volumetric criterion of the left matrix
        /// </summary>
        public double CalculateVolumetricCriterion()
        {
            var det = leftMatrix.Determinant();

            double product = 1;
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < size; j++)
                {
                    sum += leftMatrix[i, j] * leftMatrix[i, j];
                }
                product *= Math.Sqrt(sum);
            }

            return det / product;
        }
    }
}
