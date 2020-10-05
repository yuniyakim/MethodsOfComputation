using LaboratoryWork2;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace LaboratoryWork2Tests
{
    public class LagrangeInterpolationTests
    {
        private LagrangeInterpolation lagrangeInterpolation = new LagrangeInterpolation();
        private double left = 0;
        private double right = 1;
        private double point = 0.65;
        private int amountOfNodes = 15; // m
        private int powerOfPolynomial = 7; // n
        private double Function(double x) => 1 - Math.Exp(-2 * x);

        [Test]
        public void NodesTableTest()
        {
            var table1 = lagrangeInterpolation.NodesTable(left, right, amountOfNodes);
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

            var table2 = lagrangeInterpolation.NodesTable(left, right, 10);
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
            var table = lagrangeInterpolation.NodesTable(left, right, amountOfNodes);
            var sortedTable = lagrangeInterpolation.SortedNodesTable(table, point, powerOfPolynomial);

            Assert.IsTrue(sortedTable.Contains(new KeyValuePair<double, double>(0.6, 0.6988057880877978)));
            Assert.IsTrue(sortedTable.Contains(new KeyValuePair<double, double>(0.4, 0.5506710358827784)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(1, 0.8646647167633873)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(0.9, 0.9333333333333333)));
            Assert.IsFalse(sortedTable.Contains(new KeyValuePair<double, double>(0.06666666666666667, 0.12482668095705252)));
        }

        [Test]
        public void LagrangePolynomialValueTest()
        {
            var table = lagrangeInterpolation.NodesTable(left, right, amountOfNodes);
            var orderedTable = lagrangeInterpolation.SortedNodesTable(table, point, powerOfPolynomial);

            Assert.That(lagrangeInterpolation.LagrangePolynomialValue(orderedTable), Is.EqualTo(Function(point)).Within(Math.Pow(10, -10)));
        }
    }
}
