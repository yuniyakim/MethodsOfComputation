using System;
using NUnit.Framework;
using LaboratoryWork1;
using System.Collections.Generic;

namespace LaboratoryWork1Tests
{
    public class Tests
    {
        private static readonly double epsilon = Math.Pow(10, -5);
        private static readonly double accuracy = Math.Pow(10, -1);
        private static readonly double left = -10;
        private static readonly double right = 2;

        [Test]
        public void SeparateRootsTest()
        {
            var intervals1 = Program.SeparateRoots(left, right, 1200);
            var intervals2 = Program.SeparateRoots(left, right, 500);
            var intervals3 = Program.SeparateRoots(left, right, 800);
            var intervals4 = Program.SeparateRoots(left, right, 1000);

            for (var i = 0; i < intervals1.Count; i++)
            {
                Assert.That(intervals1[i].Item1, Is.EqualTo(intervals2[i].Item1).Within(accuracy));
                Assert.That(intervals2[i].Item1, Is.EqualTo(intervals3[i].Item1).Within(accuracy));
                Assert.That(intervals3[i].Item1, Is.EqualTo(intervals4[i].Item1).Within(accuracy));
                Assert.That(intervals1[i].Item2, Is.EqualTo(intervals2[i].Item2).Within(accuracy));
                Assert.That(intervals2[i].Item2, Is.EqualTo(intervals3[i].Item2).Within(accuracy));
                Assert.That(intervals3[i].Item2, Is.EqualTo(intervals4[i].Item2).Within(accuracy));
            }
        }

        [Test]
        public void FindRootTest()
        {
            var intervals = Program.SeparateRoots(left, right, 500);

            var bisectionList = new List<double>();
            var newtonList = new List<double>();
            var modifiedNewtonList = new List<double>();
            var secantList = new List<double>();

            foreach (var interval in intervals)
            {
                bisectionList.Add(Program.BisectionFindRoot(interval.Item1, interval.Item2));
                newtonList.Add(Program.NewtonFindRoot(interval.Item1, interval.Item2, 1));
                modifiedNewtonList.Add(Program.ModifiedNewtonFindRoot(interval.Item1, interval.Item2));
                secantList.Add(Program.SecantFindRoot(interval.Item1, interval.Item2));
            }

            for (var i = 0; i < bisectionList.Count; i++)
            {
                Assert.That(bisectionList[i], Is.EqualTo(newtonList[i]).Within(epsilon));
                Assert.That(newtonList[i], Is.EqualTo(modifiedNewtonList[i]).Within(epsilon));
                Assert.That(modifiedNewtonList[i], Is.EqualTo(secantList[i]).Within(epsilon));
            }
        }
    }
}
