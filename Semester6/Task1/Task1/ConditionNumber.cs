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
        private Matrix matrix;
        private Vector vector;
        private Vector exactSolution;

        public int size { get; private set; }

        public ConditionNumber() { }

        /// <summary>
        /// Condition number's constructor
        /// </summary>
        /// <param name="matrix">Given matrix</param>
        /// <param name="vector">Given vector</param>
        /// <param name="exactSolution">Given exact solution</param>
        public ConditionNumber(Matrix matrix, Vector vector, Vector exactSolution)
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

        public void Start()
        {
            var equations = new List<(Matrix, Vector, Vector)>();
            Matrix leftMatrix = new Matrix(2, 2);
            leftMatrix[0, 0] = -400.6;
            leftMatrix[0, 1] = 199.8;
            leftMatrix[1, 0] = 1198.8;
            leftMatrix[1, 1] = -600.4;
            var rightMatrix = new Vector(new double[] { 200, -600 });
            var exactSolution = new Vector(new double[] { -0.2, 0.6 });
            equations.Add((leftMatrix, rightMatrix, exactSolution));

            foreach (var equation in equations)
            {
                var conditionNumber = new ConditionNumber(equation.Item1, equation.Item2, equation.Item3);
                Console.WriteLine("Left matrix is");
                Console.WriteLine(equation.Item1.MatrixToString());
                Console.WriteLine("Right matrix is");
                Console.WriteLine(equation.Item2[0]);
                Console.WriteLine(equation.Item2[1]);
                Console.WriteLine();
                Console.WriteLine("Exact solution is");
                Console.WriteLine(equation.Item3[0]);
                Console.WriteLine(equation.Item3[1]);

                var spectralCriterion = conditionNumber.CalculateSpectralCriterion();
                var volumetricCriterion = conditionNumber.CalculateVolumetricCriterion();
                var angleCriterion = conditionNumber.CalculateAngleCriterion();
                Console.WriteLine();
                Console.WriteLine($"Spectral criterion is {spectralCriterion}.");
                Console.WriteLine($"Volumetric criterion is {volumetricCriterion}.");
                Console.WriteLine($"Angle criterion is {angleCriterion}.");

                var variedSolution2 = conditionNumber.CalculateVariedSolution(0.01);
                var variedSolution5 = conditionNumber.CalculateVariedSolution(0.00001);
                var variedSolution8 = conditionNumber.CalculateVariedSolution(0.00000001);
                var difference2 = equation.Item3 - variedSolution2;
                var difference5 = equation.Item3 - variedSolution5;
                var difference8 = equation.Item3 - variedSolution8;
                double delta2 = 0;
                double delta5 = 0;
                double delta8 = 0;
                for (var i = 0; i < conditionNumber.size; i++)
                {
                    delta2 += difference2[i] * difference2[i];
                    delta5 += difference5[i] * difference5[i];
                    delta8 += difference8[i] * difference8[i];
                }
                delta2 = Math.Sqrt(delta2);
                delta5 = Math.Sqrt(delta5);
                delta8 = Math.Sqrt(delta8);

                Console.WriteLine();
                Console.WriteLine($"|x - x˜| is {delta2} when variation is 0.01.");
                Console.WriteLine($"|x - x˜| is {delta5} when variation is 0.00001.");
                Console.WriteLine($"|x - x˜| is {delta8} when variation is 0.00000001.");
            }
        }
    }
}
