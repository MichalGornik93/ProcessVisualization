using TankSimulation.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TankSimulation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        TankSimulationPlcService _tankSimulationPlcService;
        #region Properties
        private double _tankLevel;
       public double TankLevel
        {
            get => _tankLevel;
            set => SetProperty(ref _tankLevel, Math.Round(value, 2, MidpointRounding.AwayFromZero));
        }

        private double _flowSpeed;
        public double FlowSpeed
        {
            get => _flowSpeed;
            set => SetProperty(ref _flowSpeed, Math.Round(value, 2, MidpointRounding.AwayFromZero));
        }

        private double _pumpsSpeed;
        public double PumpsSpeed
        {
            get => _pumpsSpeed;
            set => SetProperty(ref _pumpsSpeed, Math.Round(value, 2, MidpointRounding.AwayFromZero));
        }

        private bool _autoState;
        public bool AutoState
        {
            get => _autoState;
            set => SetProperty(ref _autoState, value);
        }

        private bool _pumpsState;
        public bool PumpsState
        {
            get => _pumpsState;
            set => SetProperty(ref _pumpsState, value);
        }

        private bool _flowState;
        public bool FlowState
        {
            get => _flowState;
            set => SetProperty(ref _flowState, value);
        }
        #endregion
        public Command StartPumpManualCommand { get; }
        private async Task ExecuteStartPumpManualCommand()
        {
            await _tankSimulationPlcService.StartPumpManual();
        }
        public Command StartFlowManualCommand { get; }
        private async Task ExecuteStartFlowManualCommand()
        {
            await _tankSimulationPlcService.StartFlowManual();
        }
        
        public Command StartAutoCommand { get; }
        private async Task ExecuteStartAutoCommand()
        {
            await _tankSimulationPlcService.StartAuto();
        }

        public Command StopAutoCommand { get; }
        private async Task ExecuteStopAutoCommand()
        {
            await _tankSimulationPlcService.StopAuto();
        }

        public MainViewModel()
        {
            _tankSimulationPlcService = new TankSimulationPlcService();

            try
            {
                _tankSimulationPlcService.Connect("192.168.0.89", 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
            OnPlcValuesRefreshed(null, null);
            _tankSimulationPlcService.ValuesRefreshed += OnPlcValuesRefreshed;

            StartPumpManualCommand = new Command(async () => await ExecuteStartPumpManualCommand());
            StartFlowManualCommand = new Command(async () => await ExecuteStartFlowManualCommand());
            StartAutoCommand = new Command(async () => await ExecuteStartAutoCommand());
            StopAutoCommand = new Command(async () => await ExecuteStopAutoCommand());
        }

        public void SetPumpsSpeed(double value)
        {
            _tankSimulationPlcService.SetPumpsSpeed(value);
        }

        public void SetFlowSpeed(double value)
        {
            _tankSimulationPlcService.SetFlowSpeed(value);
        }

        private void OnPlcValuesRefreshed(object sender, EventArgs e)
        {
            TankLevel = _tankSimulationPlcService.TankLevel;
            PumpsSpeed = _tankSimulationPlcService.PumpsSpeed;
            FlowSpeed = _tankSimulationPlcService.FlowSpeed;
            AutoState = _tankSimulationPlcService.AutoState;
            PumpsState = _tankSimulationPlcService.PumpsState;
            FlowState = _tankSimulationPlcService.FlowState;
        }
    }
}