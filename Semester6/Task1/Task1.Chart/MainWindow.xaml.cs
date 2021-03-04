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
            matrix4[0, 0] = -402.5;
            matrix4[0, 1] = 200.5;
            matrix4[1, 0] = 1203;
            matrix4[1, 1] = -603;
            var vector4 = new DotNumerics.LinearAlgebra.Vector(new double[] { -240.9, 723 });
            var exactSolution4 = new DotNumerics.LinearAlgebra.Vector(new double[] { 0.2, -0.8 });
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
            var vector7 = new DotNumerics.LinearAlgebra.Vector(new double[] { 2, 9, 4, 7, 11, 9, 2 });
            var exactSolution7 = new DotNumerics.LinearAlgebra.Vector(new double[] { 5279 / 420, 3743 / 420, 17771 / 2520, 47 / 8, 139999 / 27720, 30743 / 6930, 129697 / 32760 });
            equations.Add((matrix7, vector7, exactSolution7));

            for (var i = 0; i < 6; i++)
            {
                var conditionNumber = new ConditionNumber(equations[i].Item1, equations[i].Item2, equations[i].Item3);

                var spectralCriterion = conditionNumber.CalculateSpectralCriterion();
                var volumetricCriterion = conditionNumber.CalculateVolumetricCriterion();
                var angleCriterion = conditionNumber.CalculateAngleCriterion();

                var variedSolution2 = conditionNumber.CalculateVariedSolution(0.01);
                var variedSolution5 = conditionNumber.CalculateVariedSolution(0.00001);
                var difference2 = equations[i].Item3 - variedSolution2;
                var difference5 = equations[i].Item3 - variedSolution5;
                double delta2 = 0;
                double delta5 = 0;
                for (var j = 0; j < equations[i].Item1.RowCount; j++)
                {
                    delta2 += difference2[j] * difference2[j];
                    delta5 += difference5[j] * difference5[j];
                }
                delta2 = Math.Sqrt(delta2);
                delta5 = Math.Sqrt(delta5);

                // red dots
                series[i].Points.Add(new ScatterPoint(spectralCriterion, delta2, 3, 1));
                // black dots
                series[i].Points.Add(new ScatterPoint(spectralCriterion, delta5, 3, 0));
                Model.Series.Add(series[i]);
            }
        }

        public PlotModel Model { get; private set; }
    }
}
