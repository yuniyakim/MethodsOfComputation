using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra;

namespace Task7
{
    public static class GridMethod
    {
        private static (List<double>, List<double>, List<double>, List<double>) CalculateCoefs(double a, double b, List<Func<double, double>> functions, List<double> conditions, double h)
        {
            var alpha0 = conditions[0];
            var alpha1 = conditions[1];
            var beta0 = conditions[2];
            var beta1 = conditions[3];
            var Ac = conditions[4];
            var Bc = conditions[5];
            var k = functions[0];
            var p = functions[1];
            var q = functions[2];
            var f = functions[3];
            var n = (int)Math.Round((b - a) / h);
            var x = new List<double>();
            for (var i = 0; i < n + 1; i++)
            {
                x.Add(a + i * h);
            }
            var A = new List<double>();
            A.Add(0);
            var B = new List<double>();
            B.Add(h * alpha0 - alpha1);
            var C = new List<double>();
            C.Add(alpha1);
            var D = new List<double>();
            D.Add(h * Ac);
            for (var i = 1; i < n; i++)
            {
                A.Add(2 * k(x[i]) - h * p(x[i]));
                B.Add(-4 * k(x[i]) + 2 * h * h * q(x[i]));
                C.Add(2 * k(x[i]) + h * p(x[i]));
                D.Add(2 * h * h * f(x[i]));
            }
            A.Add(-beta1);
            B.Add(h * beta0 + beta1);
            C.Add(0);
            D.Add(h * Bc);
            return (A, B, C, D);
        }

        private static List<double> Solve(double a, double b, List<Func<double, double>> functions, List<double> conditions, double h)
        {
            var n = (int)Math.Round((b - a) / h);
            var coefs = CalculateCoefs(a, b, functions, conditions, h);
            var A = coefs.Item1;
            var B = coefs.Item2;
            var C = coefs.Item3;
            var D = coefs.Item4;
            var s = new List<double>(n + 1);
            var t = new List<double>(n + 1);
            var u = new List<double>(n + 1);
            s.Add(-C[0] / B[0]);
            t.Add(D[0] / B[0]);
            u.Add(0);
            for (var i = 1; i < n + 1; i++)
            {
                s.Add(-C[i] / (A[i] * s[i - 1] + B[i]));
                t.Add((D[i] - A[i] * t[i - 1]) / (A[i] * s[i - 1] + B[i]));
                u.Add(0);
            }
            u[n] = t[n];
            for (var i = n - 1; i > -1; i--)
            {
                u[i] = s[i] * u[i + 1] + t[i];
            }
            return u;
        }

        public static (List<double>, List<double>, double, double) Grid(double a, double b, List<Func<double, double>> functions, List<double> conditions, double h, double epsilon)
        {
            var coef = 2;
            var k = 0;
            var v2 = Solve(a, b, functions, conditions, h);
            while (true)
            {
                k++;
                var v1 = v2.GetRange(0, v2.Count);
                v2 = Solve(a, b, functions, conditions, h / Math.Pow(coef, k));
                var error = new List<double>();
                for (var i = 0; i < v1.Count; i++)
                {
                    error.Add((v2[2 * i] - v1[i]) / (Math.Pow(coef, 1) - 1));
                }
                if (Vector<double>.Build.DenseOfEnumerable(error).L2Norm() < epsilon)
                {
                    for (var i = 0; i < error.Count; i++)
                    {
                        if (i % 2 == 0)
                        {
                            v2[2 * i] += error[i];
                        }
                        else
                        {
                            v2[i] += (error[i - 1] + error[i + 1]) / 2;
                        }
                    }
                    var x = new List<double>(v2.Count);
                    for (var i = 0; i < v2.Count; i++)
                    {
                        x.Add(a + i * h / Math.Pow(coef, k));
                    }
                    return (x, v2, h / Math.Pow(coef, k), k);
                }
            }
        }
    }
}
