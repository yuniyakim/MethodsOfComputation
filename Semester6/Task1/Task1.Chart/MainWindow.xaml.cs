using System;
using System.Collections.Generic;
using System.Windows;
using DotNumerics.LinearAlgebra;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace Task1.Chart
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
            Model = new PlotModel { Title = "Chart" };
            Model.Axes.Add(new LinearColorAxis { Position = AxisPosition.None, Minimum = 0.1, Maximum = 0.9, HighColor = OxyColors.Red, LowColor = OxyColors.Black });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Condition number", FontSize = 14 });
            Model.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Solutions difference", FontSize = 14 });

            var series = new ScatterSeries[8];
            series[0] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[1] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[2] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[3] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[4] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[5] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[6] = new ScatterSeries { MarkerType = MarkerType.Circle };
            series[7] = new ScatterSeries { MarkerType = MarkerType.Circle };

            var equations = new List<(Matrix, DotNumerics.LinearAlgebra.Vector, DotNumerics.LinearAlgebra.Vector)>();
            var matrix1 = new Matrix(2, 2);
            matrix1[0, 0] = -400.6;
            matrix1[0, 1] = 199.8;
            matrix1[1, 0] = 1198.8;
            matrix1[1, 1] = -600.4;
            var vector1 = new DotNumerics.LinearAlgebra.Vector(new double[] { 200, -600 });
            var exactSolution1 = new DotNumerics.LinearAlgebra.Vector(new double[] { -0.2, 0.6 });
            equations.Add((matrix1, vector1, exactSolution1));

            var matrix2 = new Matrix(2, 2);
            matrix2[0, 0] = -401.98;
            matrix2[0, 1] = 200.34;
            matrix2[1, 0] = 1202.04;
            matrix2[1, 1] = -602.32;
            var vector2 = new DotNumerics.LinearAlgebra.Vector(new double[] { 722.264, -2166.272 });
            var exactSolution2 = new DotNumerics.LinearAlgebra.Vector(new double[] { -0.8, 2 });
            equations.Add((matrix2, vector2, exactSolution2));

            var matrix3 = new Matrix(2, 2);
            matrix3[0, 0] = -400.94;
            matrix3[0, 1] = 200.2;
            matrix3[1, 0] = 1200.12;
            matrix3[1, 1] = -600.96;
            var vector3 = new DotNumerics.LinearAlgebra.Vector(new double[] { -160.268, 480.408 });
            var exactSolution3 = new DotNumerics.LinearAlgebra.Vector(new double[] { 0.2, -0.4 });
            equations.Add((matrix3, vector3, exactSolution3));

            var matrix4 = new Matrix(2, 2);
            matrix4[0, 0] = -41;
            matrix4[0, 1] = 127;
            matrix4[1, 0] = 113;
            matrix4[1, 1] = -60;
            var vector4 = new DotNumerics.LinearAlgebra.Vector(new double[] { -787, 1589 });
            var exactSolution4 = new DotNumerics.LinearAlgebra.Vector(new double[] { 13, -2 });
            equations.Add((matrix4, vector4, exactSolution4));

            var matrix5 = new Matrix(2, 2);
            matrix5[0, 0] = -402.9;
            matrix5[0, 1] = 200.7;
            matrix5[1, 0] = 1204.2;
            matrix5[1, 1] = -603.6;
            var vector5 = new DotNumerics.LinearAlgebra.Vector(new double[] { 201, -603 });
            var exactSolution5 = new DotNumerics.LinearAlgebra.Vector(new double[] { -0.2, 0.6 });
            equations.Add((matrix5, vector5, exactSolution5));

            // bad condition matrix
            var matrix6 = new Matrix(2, 2);
            matrix6[0, 0] = 1;
            matrix6[0, 1] = 0.99;
            matrix6[1, 0] = 0.99;
            matrix6[1, 1] = 0.98;
            var vector6 = new DotNumerics.LinearAlgebra.Vector(new double[] { 2, 2 });
            var exactSolution6 = new DotNumerics.LinearAlgebra.Vector(new double[] { 200, -200 });
            equations.Add((matrix6, vector6, exactSolution6));

            // Hilbert matrix
            var matrix7 = new Matrix(7, 7);
            for (var i = 0; i < 7; i++)
            {
                for (var j = 0; j < 7; j++)
                {
                    matrix7[i, j] = 1;
                    matrix7[i, j] /= 1 + i + j;
                }
            }

            var vector7 = new DotNumerics.LinearAlgebra.Vector(new double[] { 1, 1, 1, 1, 1, 1, 1 });
            vector7[0] = vector7[0] * 5699 / 420;
            vector7[1] = vector7[1] * 4103 / 420;
            vector7[2] = vector7[2] * 19661 / 2520;
            vector7[3] = vector7[3] * 157 / 24;
            vector7[4] = vector7[4] * 156631 / 27720;
            vector7[5] = vector7[5] * 34523 / 6930;
            vector7[6] = vector7[6] * 146077 / 32760;
            var exactSolution7 = new DotNumerics.LinearAlgebra.Vector(new double[] { 2, 9, 4, 7, 11, 9, 2 });
            equations.Add((matrix7, vector7, exactSolution7));

            for (var i = 0; i < 7; i++)
            {
                var conditionNumber = new ConditionNumber(equations[i].Item1, equations[i].Item2, equations[i].Item3);

                var spectralCriterion = conditionNumber.CalculateSpectralCriterion();
                var volumetricCriterion = conditionNumber.CalculateVolumetricCriterion();
                var angleCriterion = conditionNumber.CalculateAngleCriterion();

                var variedSolution1 = conditionNumber.CalculateVariedSolution(0.1);
                var variedSolution2 = conditionNumber.CalculateVariedSolution(0.01);
                var variedSolution3 = conditionNumber.CalculateVariedSolution(0.001);
                var difference1 = equations[i].Item3 - variedSolution1;
                var difference2 = equations[i].Item3 - variedSolution2;
                var difference3 = equations[i].Item3 - variedSolution3;
                double delta1 = 0;
                double delta2 = 0;
                double delta3 = 0;
                for (var j = 0; j < equations[i].Item1.RowCount; j++)
                {
                    delta1 += difference1[j] * difference1[j];
                    delta2 += difference2[j] * difference2[j];
                    delta3 += difference3[j] * difference3[j];
                }
                delta1 = Math.Sqrt(delta1);
                delta2 = Math.Sqrt(delta2);
                delta3 = Math.Sqrt(delta3);

                // red dots
                series[i].Points.Add(new ScatterPoint(spectralCriterion, delta1, 3, 1));
                // green dots
                series[i].Points.Add(new ScatterPoint(spectralCriterion, delta2, 3, 0.4));
                // black dots
                series[i].Points.Add(new ScatterPoint(spectralCriterion, delta3, 3, 0));
                Model.Series.Add(series[i]);
            }
        }

        public PlotModel Model { get; private set; }
    }
}
