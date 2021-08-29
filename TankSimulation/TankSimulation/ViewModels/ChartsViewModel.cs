using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using TankSimulation.Helpers;
using TankSimulation.Services;

namespace TankSimulation.ViewModels
{
    class ChartsViewModel : BaseViewModel
    {
        TankSimulationPlcService _tankSimulationPlcService;

        private List<ChartEntry> _tankLevelEntries;

        private List<ChartEntry> _flowEntries;

        private List<ChartEntry> _pumpEntries;

        private LineChart _tankLevelChart;
        public LineChart TankLevelChart
        {
            get => _tankLevelChart;
            set => SetProperty(ref _tankLevelChart, value);
        }

        private LineChart _pumpChart;
        public LineChart PumpChart
        {
            get => _pumpChart;
            set => SetProperty(ref _pumpChart, value);
        }

        private LineChart _flowChart;
        public LineChart FlowChart
        {
            get => _flowChart;
            set => SetProperty(ref _flowChart, value);
        }

        public ChartsViewModel() //TODO: Validation
        {
            _tankLevelEntries = new List<ChartEntry>();
            _flowEntries = new List<ChartEntry>();
            _pumpEntries = new List<ChartEntry>();
        
            _tankSimulationPlcService = new TankSimulationPlcService();
            _tankSimulationPlcService.Connect("192.168.0.89", 0, 0);  //TODO: Ip like resource
            OnPlcValuesRefreshed(null, null);

            _tankSimulationPlcService.ValuesRefreshed += OnPlcValuesRefreshed;
        }

        private void OnPlcValuesRefreshed(object sender, EventArgs e)
        {
            if (_tankLevelEntries.Count <= 4)
            {
                _tankLevelEntries.Add(new ChartEntry(_tankSimulationPlcService.TankLevel)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.TankLevel, 2, MidpointRounding.AwayFromZero).ToString()+ " m",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                _tankLevelEntries = _tankLevelEntries.ShiftRight(1);
                _tankLevelEntries.RemoveAt(0);
                _tankLevelEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.TankLevel)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.TankLevel, 2, MidpointRounding.AwayFromZero).ToString() + " m",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }

            if (_pumpEntries.Count <=4)
            {
                _pumpEntries.Add(new ChartEntry(_tankSimulationPlcService.PumpsSpeed)
                {
                    Color = SKColor.Parse("#CC0000"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.PumpsSpeed, 2, MidpointRounding.AwayFromZero).ToString()+ "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                _pumpEntries = _pumpEntries.ShiftRight(1);
                _pumpEntries.RemoveAt(0);
                _pumpEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.PumpsSpeed)
                {
                    Color = SKColor.Parse("#CC0000"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.PumpsSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }

            if(_flowEntries.Count <=4)
            {
                _flowEntries.Add(new ChartEntry(_tankSimulationPlcService.FlowSpeed)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.FlowSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                _flowEntries = _flowEntries.ShiftRight(1);
                _flowEntries.RemoveAt(0);
                _flowEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.FlowSpeed)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.FlowSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }

            TankLevelChart = new LineChart
            {
                Entries = _tankLevelEntries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Horizontal,
                MaxValue = 5,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                AnimationDuration = new TimeSpan(0)
            };

            PumpChart = new LineChart
            {
                Entries = _pumpEntries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Horizontal,
                MaxValue = 5,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                AnimationDuration = new TimeSpan(0)
            };

            FlowChart = new LineChart
            {
                Entries = _flowEntries,
                LabelTextSize = 30f,
                LabelOrientation = Orientation.Horizontal,
                MaxValue = 5,
                ValueLabelOrientation = Orientation.Horizontal,
                IsAnimated = false,
                AnimationDuration = new TimeSpan(0)
            };

            Thread.Sleep(2000); //TODO: Interval like resource
        }
    };
}

