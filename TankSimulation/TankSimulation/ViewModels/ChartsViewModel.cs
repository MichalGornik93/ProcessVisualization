using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading;
using TankSimulation.Helpers;
using TankSimulation.Services;
using Xamarin.Forms;

namespace TankSimulation.ViewModels
{
    class ChartsViewModel : BaseViewModel
    {
        Page page;
        TankSimulationPlcService _tankSimulationPlcService;

        private List<ChartEntry> _tankLevelEntries;

        private List<ChartEntry> _flowSpeedEntries;

        private List<ChartEntry> _pumpSpeedEntries;

        private LineChart _tankLevelChart;
        public LineChart TankLevelChart
        {
            get => _tankLevelChart;
            set => SetProperty(ref _tankLevelChart, value);
        }

        private LineChart _pumpSpeedChart;
        public LineChart PumpSpeedChart
        {
            get => _pumpSpeedChart;
            set => SetProperty(ref _pumpSpeedChart, value);
        }

        private LineChart _flowSpeedChart;
        public LineChart FlowSpeedChart
        {
            get => _flowSpeedChart;
            set => SetProperty(ref _flowSpeedChart, value);
        }

        public ChartsViewModel(Page page) //TODO: Validation
        {
            _tankLevelEntries = new List<ChartEntry>();
            _flowSpeedEntries = new List<ChartEntry>();
            _pumpSpeedEntries = new List<ChartEntry>();

            this.page = page;

            _tankSimulationPlcService = new TankSimulationPlcService();

            try
            {
                _tankSimulationPlcService.Connect("192.168.0.89", 0, 0);
            }
            catch
            {
                page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
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
                    ValueLabel = Math.Round(_tankSimulationPlcService.TankLevel, 2, MidpointRounding.AwayFromZero).ToString() + " m",
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

            if (_pumpSpeedEntries.Count <= 4)
            {
                _pumpSpeedEntries.Add(new ChartEntry(_tankSimulationPlcService.RealPumpsSpeed)
                {
                    Color = SKColor.Parse("#CC0000"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.RealPumpsSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                _pumpSpeedEntries = _pumpSpeedEntries.ShiftRight(1);
                _pumpSpeedEntries.RemoveAt(0);
                _pumpSpeedEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.RealPumpsSpeed)
                {
                    Color = SKColor.Parse("#CC0000"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.RealPumpsSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }

            if (_flowSpeedEntries.Count <= 4)
            {
                _flowSpeedEntries.Add(new ChartEntry(_tankSimulationPlcService.RealFlowSpeed)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.RealFlowSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            else
            {
                _flowSpeedEntries = _flowSpeedEntries.ShiftRight(1);
                _flowSpeedEntries.RemoveAt(0);
                _flowSpeedEntries.Insert(0, new ChartEntry(_tankSimulationPlcService.RealFlowSpeed)
                {
                    Color = SKColor.Parse("#09C"),
                    ValueLabel = Math.Round(_tankSimulationPlcService.RealFlowSpeed, 2, MidpointRounding.AwayFromZero).ToString() + "m³/h",
                    Label = $"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second} "
                });
            }
            try
            {
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

                PumpSpeedChart = new LineChart
                {
                    Entries = _pumpSpeedEntries,
                    LabelTextSize = 30f,
                    LabelOrientation = Orientation.Horizontal,
                    MaxValue = 5,
                    ValueLabelOrientation = Orientation.Horizontal,
                    IsAnimated = false,
                    AnimationDuration = new TimeSpan(0)
                };

                FlowSpeedChart = new LineChart
                {
                    Entries = _flowSpeedEntries,
                    LabelTextSize = 30f,
                    LabelOrientation = Orientation.Horizontal,
                    MaxValue = 5,
                    ValueLabelOrientation = Orientation.Horizontal,
                    IsAnimated = false,
                    AnimationDuration = new TimeSpan(0)
                };
            }
            catch
            {
                page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }

            Thread.Sleep(2000); //TODO: Interval like resource
        }
    };
}

