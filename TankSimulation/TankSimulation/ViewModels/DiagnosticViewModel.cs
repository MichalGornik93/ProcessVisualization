using System;
using System.Threading;
using System.Threading.Tasks;
using TankSimulation.Services;
using Xamarin.Forms;

namespace TankSimulation.ViewModels
{
    class DiagnosticViewModel : BaseViewModel
    {
        #region Properties

        TankSimulationPlcService _tankSimulationPlcService;
        Page page;

        private string _plcIpAddress = "192.168.0.89";
        public string PlcIpAddress
        {
            get => _plcIpAddress;
            set => SetProperty(ref _plcIpAddress, value);
        }

        private ConnectionStates _connectionState;
        public ConnectionStates ConnectionState
        {
            get => _connectionState;
            set => SetProperty(ref _connectionState, value);
        }

        private TimeSpan _scanTime;
        public TimeSpan ScanTime
        {
            get => _scanTime;
            set => SetProperty(ref _scanTime, value);
        }
        #endregion

        #region Commands
        public Command ConnectCommand { get; }
        private async Task ExecuteConnectCommand()
        {
            try
            {
                _tankSimulationPlcService.Disconnect();
                _tankSimulationPlcService.Connect(PlcIpAddress, 0, 0);
            }
            catch (Exception)
            {
                await page.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
        }

        public Command DisconnectCommand { get; }
        private async Task ExecuteDisconnectCommand()
        {
            ScanTime = TimeSpan.Zero;
            await _tankSimulationPlcService.Disconnect();   
        }
        #endregion
        public DiagnosticViewModel(Page page)
        {
            this.page = page;

            _tankSimulationPlcService = new TankSimulationPlcService();

            ConnectCommand = new Command(async () => await ExecuteConnectCommand());
            DisconnectCommand = new Command(async () => await ExecuteDisconnectCommand());
            OnPlcValuesRefreshed(null, null);
            _tankSimulationPlcService.ValuesRefreshed += OnPlcValuesRefreshed;
        }

        private void OnPlcValuesRefreshed(object sender, EventArgs e)
        {
            ConnectionState = _tankSimulationPlcService.ConnectionState;
            ScanTime = _tankSimulationPlcService.ScanTime;
        }
    }
}
