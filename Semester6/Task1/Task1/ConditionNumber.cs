using System;
using System.Linq;
using System.Collections.Generic;
using DotNumerics.LinearAlgebra;

namespace Task1
{
    /// <summary>
    /// Condition number of a matrix
    /// </summary>
    public class ConditionNumber
    {
        private DotNumerics.LinearAlgebra.Matrix matrix;
        private Vector vector;
        private int size;
        private Vector exactSolution;

        //public double SpectralCriterion { get; private set; }
        //public double VolumetricCriterion { get; private set; }
        //public double AngleCriterion { get; private set; }

        /// <summary>
        /// Condition number's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        /// <param name="vector">Given vector</param>
        /// <param name="exactSolution">Given exact solution</param>
        public ConditionNumber(DotNumerics.LinearAlgebra.Matrix matrix, Vector vector, Vector exactSolution)
        {
            this.matrix = matrix;
            this.vector = vector;
            size = matrix.RowCount;
            this.exactSolution = exactSolution;
        }

        /// <summary>
        /// Calculates spectral criterion of the left matrix
        /// </summary>
        /// <returns>Spectral criterion's value</returns>
        public double CalculateSpectralCriterion()
        {
            var matrix = new Nomad.Core.Matrix(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    matrix[i, j] = this.matrix[i, j];
                }
            }

            var norm = matrix.EuclideanNorm();
            var normInverse = matrix.Inverse().EuclideanNorm();
            return norm * normInverse;
        }

        /// <summary>
        /// Calculates volumetric criterion of the left matrix
        /// </summary>
        /// <returns>Volumetric criterion's value</returns>
        public double CalculateVolumetricCriterion()
        {
            var det = matrix.Determinant();

            double product = 1;
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < size; j++)
                {
                    sum += matrix[i, j] * matrix[i, j];
                }
                product *= Math.Sqrt(sum);
            }

            return det / product;
        }

        /// <summary>
        /// Calculates angle criterion of the left matrix
        /// </summary>
        /// <returns>Angle criterion's value</returns>
        public double CalculateAngleCriterion()
        {
            var inverse = matrix.Inverse();
            var products = new double[size];
            for (var i = 0; i < size; i++)
            {
                double sum = 0;
                for (var j = 0; j < size; j++)
                {
                    sum += Math.Abs(matrix[i, j]) * Math.Abs(inverse[j, i]);
                }
                products[i] = sum;
            }

            return products.Max();
        }

        /// <summary>
        /// Varies matrix and vector in equation and calculates new solution
        /// </summary>
        /// <param name="variation">Variation's value</param>
        /// <returns>Solution of varied equation</returns>
        public Vector CalculateVariedSolution(double variation)
        {
            var variedMatrix = matrix;
            var variedVector = vector;
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                {
                    variedMatrix[i, j] += variation;
                }
                variedVector[i] += variation;
            }

            var solver = new LinearEquations();
            return solver.Solve(variedMatrix, variedVector);
        }
    }
}
