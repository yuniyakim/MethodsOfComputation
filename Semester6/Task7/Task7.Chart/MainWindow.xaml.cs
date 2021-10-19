using System;
using System.Collections.Generic;
using System.Windows;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Task7.Chart
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
            var functions1 = new List<Func<double, double>>();
            functions1.Add(x => -(4 - x) / (5 - 2 * x));
            functions1.Add(x => (1 - x) / 2);
            functions1.Add(x => Math.Log(3 + x) / 2);
            functions1.Add(x => 1 + x / 3);

            var functions2 = new List<Func<double, double>>();
            functions2.Add(x => -(6 + x) / (7 + 3 * x));
            functions2.Add(x => -(1 - x / 2));
            functions2.Add(x => 1 + Math.Cos(x) / 2);
            functions2.Add(x => 1 - x / 3);

            var functions3 = new List<Func<double, double>>();
            functions3.Add(x => -(5 - x) / (7 - 3 * x));
            functions3.Add(x => -(1 - x) / 2);
            functions3.Add(x => 1 + Math.Sin(x) / 2);
            functions3.Add(x => 1 / 2 + x / 2);

            var functions = new List<List<Func<double, double>>>();
            functions.Add(functions1);
            functions.Add(functions2);
            functions.Add(functions3);

            var conditions1 = new List<double>();
            conditions1.Add(1);
            conditions1.Add(0);
            conditions1.Add(1);
            conditions1.Add(0);
            conditions1.Add(0);
            conditions1.Add(0);

            var conditions2 = new List<double>();
            conditions2.Add(-2);
            conditions2.Add(1);
            conditions2.Add(0);
            conditions2.Add(1);
            conditions2.Add(0);
            conditions2.Add(0);

            var conditions3 = new List<double>();
            conditions3.Add(0);
            conditions3.Add(1);
            conditions3.Add(3);
            conditions3.Add(2);
            conditions3.Add(0);
            conditions3.Add(0);

            var conditions = new List<List<double>>();
            conditions.Add(conditions1);
            conditions.Add(conditions2);
            conditions.Add(conditions3);

            var segments = new List<(double, double)>();
            segments.Add((-1, 1));
            segments.Add((-1, 1));
            segments.Add((-1, 1));

            Model = new PlotModel { Title = "Chart" };
            Model.Axes.Add(new LinearColorAxis { Position = AxisPosition.None, Minimum = 0.1, Maximum = 0.9, HighColor = OxyColors.Red, LowColor = OxyColors.Black });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "x", FontSize = 14 });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "y", FontSize = 14 });

            var series = new ScatterSeries();

            var epsilon = 0.01;
            var i = 0;
            var solution = GridMethod.Grid(segments[i].Item1, segments[i].Item2, functions[i], conditions[i], 0.125, epsilon);
            for (var j = 0; j < solution.Item1.Count; j++)
            {
                series.Points.Add(new ScatterPoint(solution.Item1[j], solution.Item2[j], 1, 1));
            }
            Model.LegendTitle = $"Error: {epsilon}. Grid step: {solution.Item3}. Thickening steps: {solution.Item4}";
            Model.LegendPosition = LegendPosition.BottomRight;
            Model.Series.Add(series);
        }

        public PlotModel Model { get; private set; }
    }
}

