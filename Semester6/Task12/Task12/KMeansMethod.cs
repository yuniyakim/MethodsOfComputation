using System;
using System.Collections.Generic;
using System.Linq;

namespace Task12
{
    public static class KMeansMethod
    {
        private static double Euclidean((double, double) x, (double, double) y) => Math.Sqrt(Math.Pow(x.Item1 - y.Item1, 2) + 
            Math.Pow(x.Item2 - y.Item2, 2));

        private static double Manhattan((double, double) x, (double, double) y) => Math.Abs(x.Item1 - y.Item1) + Math.Abs(x.Item2 - y.Item2);

        private static List<int> Cluster(List<(double, double)> X, List<(double, double)> centers, string metric)
        {
            var clusters = new List<int>();
            foreach (var x in X)
            {
                double min = 100000;
                var minIndex = -1;
                for (var i = 0; i < centers.Count; i++)
                {
                    var r = metric == "manhattan" ? Manhattan(x, centers[i]) : Euclidean(x, centers[i]);
                    if (r < min)
                    {
                        min = r;
                        minIndex = i;
                    }
                }
                clusters.Add(minIndex);
            }
            return clusters;
        }

        private static List<(double, double)> DefineCenters(List<(double, double)> X, List<int> clusters)
        {
            var centers = new List<(double, double)>();
            var uniqueClusters = clusters.Distinct();
            for (var i = 0; i < uniqueClusters.Count(); i++)
            {
                var cluster = new List<(double, double)>();
                for (var j = 0; j < X.Count; j++)
                {
                    if (clusters[j] == i)
                    {
                        cluster.Add(X[j]);
                    }
                }
                if (cluster.Count > 0)
                {
                    double xMean = 0;
                    double yMean = 0;
                    foreach (var point in cluster)
                    {
                        xMean += point.Item1;
                        yMean += point.Item2;
                    }
                    centers.Add((xMean / cluster.Count, yMean / cluster.Count));
                }
            }
            return centers;
        }

        public static (List<int>, List<(double, double)>, int) KMeans(List<(double, double)> X, List<(double, double)> centers, string metric)
        {
            var amount = 0;
            while (true)
            {
                amount++;
                var clusters = Cluster(X, centers, metric);
                centers = DefineCenters(X, clusters);
                var flag = true;
                var newClusters = Cluster(X, centers, metric);
                for (var i = 0; i < clusters.Count; i++)
                {
                    if (clusters[i] != newClusters[i])
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    return (clusters, centers, amount);
                }
            }
        }
    }
}
