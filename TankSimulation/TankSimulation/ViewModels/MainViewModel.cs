using TankSimulation.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TankSimulation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Properties

        TankSimulationPlcService _tankSimulationPlcService;
        Page page;

        private double _tankLevel;
        public double TankLevel
        {
            get => _tankLevel;
            set => SetProperty(ref _tankLevel, Math.Round(value, 2, MidpointRounding.AwayFromZero));
        }

        private double _parameterFlowSpeed;
        public double ParameterFlowSpeed
        {
            get => _parameterFlowSpeed;
            set => SetProperty(ref _parameterFlowSpeed, Math.Round(value, 2, MidpointRounding.AwayFromZero));
        }

        private double _parameterPumpsSpeed;
        public double ParameterPumpsSpeed
        {
            get => _parameterPumpsSpeed;
            set => SetProperty(ref _parameterPumpsSpeed, Math.Round(value, 2, MidpointRounding.AwayFromZero));
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

        #region Commands
        public Command StartPumpManualCommand { get; }
        private async Task ExecuteStartPumpManualCommand()
        {
            try
            {
                await _tankSimulationPlcService.StartPumpManual();
            }
            catch (Exception)
            {
                await page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
           
        }
        public Command StartFlowManualCommand { get; }
        private async Task ExecuteStartFlowManualCommand()
        {
            try
            {
                await _tankSimulationPlcService.StartFlowManual();
            }
            catch (Exception)
            {
                await page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
        }

        public Command StartAutoCommand { get; }
        private async Task ExecuteStartAutoCommand()
        {
            try
            {
                await _tankSimulationPlcService.StartAuto();
            }
            catch (Exception)
            {
                await page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
            
        }

        public Command StopAutoCommand { get; }
        private async Task ExecuteStopAutoCommand()
        {
            try
            {
                await _tankSimulationPlcService.StopAuto();
            }
            catch (Exception)
            {
                await page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
            
        }
        #endregion
        public MainViewModel(Page page)
        {
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
            StartPumpManualCommand = new Command(async () => await ExecuteStartPumpManualCommand());
            StartFlowManualCommand = new Command(async () => await ExecuteStartFlowManualCommand());
            StartAutoCommand = new Command(async () => await ExecuteStartAutoCommand());
            StopAutoCommand = new Command(async () => await ExecuteStopAutoCommand());
            OnPlcValuesRefreshed(null, null);
            _tankSimulationPlcService.ValuesRefreshed += OnPlcValuesRefreshed;
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
            ParameterPumpsSpeed = _tankSimulationPlcService.ParameterPumpsSpeed;
            ParameterFlowSpeed = _tankSimulationPlcService.ParameterFlowSpeed;
            AutoState = _tankSimulationPlcService.AutoState;
            PumpsState = _tankSimulationPlcService.PumpsState;
            FlowState = _tankSimulationPlcService.FlowState;
        }
    }
}