using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace TankSimulation.ViewModels
{
    class ChartsViewModel : BaseViewModel
    {
        private LineChart lineChart;
        public LineChart LineChart
        {
            get => lineChart;
            set => SetProperty(ref lineChart, value);
        }

        private string[] months = new string[] { "JAN", "FRB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };

        private float[] turnoverData = new float[] { 1000, 5000, 3500, 12000, 9000, 15000, 3000, 0, 0, 0, 0, 0 };

        public ChartsViewModel()
        {
            InitData();
        }

        private void InitData()
        {
            var turnoverEntries = new List<ChartEntry>();

            int i = 0;
            foreach (var data in turnoverData)
            {
                turnoverEntries.Add(new ChartEntry(data)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = $"{data / 1000} k",
                    Label = months[i]
                });
                i++;
            }
            LineChart = new LineChart { Entries = turnoverEntries, LabelTextSize = 30f, LabelOrientation = Orientation.Horizontal };
        }
        
    };

}

