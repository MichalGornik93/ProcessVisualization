using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using TankSimulation.Helpers;
using TankSimulation.Services;

namespace TankSimulation.ViewModels
{
    class ChartsViewModel : BaseViewModel
    {
        TankSimulationPlcService _tankSimulationPlcService;

        private List<ChartEntry> tankLevelEntries;

        private LineChart tankLevelChart;
        public LineChart TankLevelChart
        {
            get => tankLevelChart;
            set => SetProperty(ref tankLevelChart, value);
        }

        public ChartsViewModel() //TODO: Validation
        {
            tankLevelEntries = new List<ChartEntry>();
            _tankSimulationPlcService = new TankSimulationPlcService();
            _tankSimulationPlcService.Connect("192.168.0.89", 0, 0);  //TODO: Ip like resource
            OnPlcValuesRefreshed(null, null);

            _tankSimulationPlcService.ValuesRefreshed += OnPlcValuesRefreshed;
        }

        private void OnPlcValuesRefreshed(object sender, EventArgs e)
        {
            if (tankLevelEntries.Count <= 4)
            {
                tankLevelEntries.Add(new ChartEntry(_tankSimulationPlcService.TankLevel)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.TankLevel, 2, MidpointRounding.AwayFromZero).ToString(),
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                tankLevelEntries = tankLevelEntries.ShiftRight(1);
                tankLevelEntries.RemoveAt(0);
                tankLevelEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.TankLevel)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.TankLevel, 2, MidpointRounding.AwayFromZero).ToString(),
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }

            TankLevelChart = new LineChart
            {
                Entries = tankLevelEntries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Vertical,
                MaxValue = 5,
                ValueLabelOrientation = Orientation.Vertical,
                IsAnimated = false,
                AnimationDuration = new TimeSpan(0)
            };
            Thread.Sleep(500); //TODO: Interval like resource
        }
    };
}

