using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Task12.Chart
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
    public class MainViewModel
    {
        public MainViewModel()
        {
            var random = new Random();
            var X = new List<(double, double)>();
            //for (var i = 0; i < 200; i++)
            //{
            //    X.Add((random.NextDouble() * 200, random.NextDouble() * 200));
            //}
            //using (var sw = new StreamWriter("list.txt"))
            //{
            //    foreach (var element in X)
            //        sw.WriteLine(element.ToString());
            //}
            using (var sr = new StreamReader("list.txt"))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();
                    line = line.Substring(1, line.Length - 2);
                    X.Add((Convert.ToDouble(line.Split(',')[0]), Convert.ToDouble(line.Split(' ')[1])));
                }
            }
            var k = 4;

            var centers = new List<List<(double, double)>>();
            centers.Add(new List<(double, double)>());
            centers.Add(new List<(double, double)>());
            for (var i = 0; i < k; i++)
            {
                centers[0].Add(X[random.Next(0, X.Count)]);
            }
            double xMin = 0;
            double xMax = 0;
            double yMin = 0;
            double yMax = 0;
            foreach (var point in X)
            {
                if (point.Item1 < xMin)
                {
                    xMin = point.Item1;
                }
                if (point.Item1 > xMax)
                {
                    xMax = point.Item1;
                }
                if (point.Item2 < yMin)
                {
                    yMin = point.Item2;
                }
                if (point.Item2 > yMax)
                {
                    yMax = point.Item2;
                }
            }
            centers[1].Add((xMin, yMin));
            centers[1].Add((xMin, yMax));
            centers[1].Add((xMax, yMin));
            centers[1].Add((xMax, yMax));

            var metrics = new List<string>();
            metrics.Add("euclidean");
            metrics.Add("manhattan");

            var centersNames = new List<string>();
            centersNames.Add("random");
            centersNames.Add("min & max");

            var centerNumber = 0;
            var metricsNumber = 0;
            var kMeans = KMeansMethod.KMeans(X, centers[centerNumber], metrics[metricsNumber]);

            Model = new PlotModel { Title = "Chart" };
            var customAxis = new RangeColorAxis { Key = "customColors" };
            customAxis.AddRange(0, 0.2, OxyColors.Green);
            customAxis.AddRange(0.2, 0.4, OxyColors.Orange);
            customAxis.AddRange(0.4, 0.6, OxyColors.Blue);
            customAxis.AddRange(0.6, 0.8, OxyColors.Red);
            customAxis.AddRange(0.8, 1, OxyColors.Black);
            Model.Axes.Add(customAxis);
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x", FontSize = 14 });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "y", FontSize = 14 });

            var series = new ScatterSeries();

            for (var i = 0; i < X.Count; i++)
            {
                series.Points.Add(new ScatterPoint(X[i].Item1, X[i].Item2, 2, 0.1 + 0.2 * kMeans.Item1[i]));
            }
            for (var i = 0; i < kMeans.Item2.Count; i++)
            {
                series.Points.Add(new ScatterPoint(kMeans.Item2[i].Item1, kMeans.Item2[i].Item2, 3, 0.9));
            }

            Model.Title = $"Metrics: {metrics[metricsNumber]}, center: {centersNames[centerNumber]}.";
            Model.Series.Add(series);
        }

        public PlotModel Model { get; private set; }
    }
}
