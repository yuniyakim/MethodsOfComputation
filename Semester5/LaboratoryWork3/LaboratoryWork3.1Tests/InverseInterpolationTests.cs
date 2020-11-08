using LaboratoryWork3_1;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LaboratoryWork3_1Tests
{
    public class InverseInterpolationTests
    {
        private InverseInterpolation interpolation = new InverseInterpolation();
        private double left = 0;
        private double right = 1;
        private double point = 0.65;
        private int amountOfNodes = 15; // m
        private int powerOfPolynomial = 7; // n
        private double Function(double x) => 1 - Math.Exp(-2 * x);

        [Test]
        public void NodesTableTest()
        {
            var table1 = interpolation.NodesTable(left, right, amountOfNodes, false);
            Assert.IsTrue(table1.ContainsKey(0));
            Assert.IsTrue(table1.ContainsKey(0.2));
            Assert.IsTrue(table1.ContainsKey(0.4));
            Assert.IsTrue(table1.ContainsKey(0.5333333333333333));
            Assert.IsTrue(table1.ContainsKey(0.6));
            Assert.IsTrue(table1.ContainsKey(0.8));
            Assert.IsTrue(table1.ContainsKey(1));

            Assert.IsTrue(table1.ContainsValue(0.12482668095705252));
            Assert.IsTrue(table1.ContainsValue(0.2340716616353513));
            Assert.IsTrue(table1.ContainsValue(0.4133537804899682));
            Assert.IsTrue(table1.ContainsValue(0.7364028618842733));
            Assert.IsTrue(table1.ContainsValue(0.8453617354507452));

            var table2 = interpolation.NodesTable(left, right, 10, false);
            Assert.IsTrue(table2.ContainsKey(0));
            Assert.IsTrue(table2.ContainsKey(0.1));
            Assert.IsTrue(table2.ContainsKey(0.2));
            Assert.IsTrue(table2.ContainsKey(0.3));
            Assert.IsTrue(table2.ContainsKey(0.4));
            Assert.IsTrue(table2.ContainsKey(0.5));
            Assert.IsTrue(table2.ContainsKey(0.6));
            Assert.IsTrue(table2.ContainsKey(0.7));
            Assert.IsTrue(table2.ContainsKey(0.8));
            Assert.IsTrue(table2.ContainsKey(0.9));
            Assert.IsTrue(table2.ContainsKey(1));
        }

        [Test]
        public void SortedNodesTableTest()
        {
            var table = interpolation.NodesTable(left, right, amountOfNodes, false);
            var sortedTable = interpolation.SortedNodesTable(table, point, powerOfPolynomial);

            Assert.IsTrue(sortedTable.Contains(new KeyValuePair<double, double>(0.6, 0.6988057880877978)));
            Assert.IsTrue(sortedTable.Contains(new KeyValuePair<double, double>(0.4, 0.5506710358827784)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(1, 0.8646647167633873)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(0.9, 0.9333333333333333)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(0.06666666666666667, 0.12482668095705252)));
        }

        [Test]
        public void NewtonPolynomialValueTest()
        {
            var table = interpolation.NodesTable(left, right, amountOfNodes, false);
            var orderedTable = interpolation.SortedNodesTable(table, point, powerOfPolynomial);
            var dividedDifferences = interpolation.DividedDifferences(orderedTable);

            Assert.That(interpolation.NewtonPolynomialValue(orderedTable, dividedDifferences, point), Is.EqualTo(Function(point)).Within(Math.Pow(10, -10)));
        }

        [Test]
        public void NewtonPolynomialValueInPointTest()
        {
            var random = new Random();
            for (var i = 0; i < 500; i++)
            {
                var currentPoint = random.NextDouble();
                Assert.That(interpolation.NewtonPolynomialValueInPoint(currentPoint, left, right), Is.EqualTo(Function(currentPoint)).Within(Math.Pow(10, -4)));
            }
        }
    }
}