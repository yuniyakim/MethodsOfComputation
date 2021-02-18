using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;
using DotNumerics.LinearAlgebra;

namespace Task1.Charts
{
    public partial class ChartForm : Form
    {
        public ChartForm()
        {
            Chart chart = new Chart();
            var series = new Series("Finance");

            // Frist parameter is X-Axis and Second is Collection of Y- Axis
            series.ChartType = SeriesChartType.Line;
            series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
            chart.Series.Add(series);

            //Chart chart = new Chart();
            //chart.Parent = this;
            //chart.Dock = DockStyle.Fill;
            //chart.ChartAreas.Add(new ChartArea("Diagram"));

            ////Создаем и настраиваем набор точек для рисования графика, в том
            ////не забыв указать имя области на которой хотим отобразить этот
            ////набор точек.
            //Series diagram = new Series("Sinus");
            //diagram.ChartType = SeriesChartType.Line;
            //diagram.ChartArea = "Diagram";
            //for (double x = -Math.PI; x <= Math.PI; x += Math.PI / 10.0)
            //{
            //    diagram.Points.AddXY(x, Math.Sin(x));
            //}


            //Добавляем созданный набор точек в Chart
            chart.Series.Add("Finance");

            var equations = new List<(Matrix, Vector, Vector)>();
            var matrix1 = new Matrix(2, 2);
            matrix1[0, 0] = -400.6;
            matrix1[0, 1] = 199.8;
            matrix1[1, 0] = 1198.8;
            matrix1[1, 1] = -600.4;
            var vector1 = new Vector(new double[] { 200, -600 });
            var exactSolution1 = new Vector(new double[] { -0.2, 0.6 });
            equations.Add((matrix1, vector1, exactSolution1));

            var matrix2 = new Matrix(2, 2);
            matrix2[0, 0] = 1;
            matrix2[0, 1] = 0.99;
            matrix2[1, 0] = 0.99;
            matrix2[1, 1] = 0.98;
            var vector2 = new Vector(new double[] { 2, 2 });
            var exactSolution2 = new Vector(new double[] { 200, -200 });
            equations.Add((matrix2, vector2, exactSolution2));

            foreach (var equation in equations)
            {
                var conditionNumber = new ConditionNumber(equation.Item1, equation.Item2, equation.Item3);

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
            }
        }
    }
}

