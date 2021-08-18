﻿using TankSimulation.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TankSimulation.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        TankSimulationPlcHelper _tankSimulationPlcHelper;

        private double _tankLevel;
       public double TankLevel
        {
            get => _tankLevel;
            set => SetProperty(ref _tankLevel, value);
        }

        private double _flowSpeed;
        public double FlowSpeed
        {
            get => _flowSpeed;
            set => SetProperty(ref _flowSpeed, value);
        }

        private double _pumpsSpeed;
        public double PumpsSpeed
        {
            get => _pumpsSpeed;
            set => SetProperty(ref _pumpsSpeed, value);
        }
        public Command StartPumpCommand { get; }
        private async Task ExecuteStartPumpCommand()
        {
            await _tankSimulationPlcHelper.StartPump();
        }
        public Command StopPumpCommand { get; }
        private async Task ExecuteStopPumpCommand()
        {
            await _tankSimulationPlcHelper.StopPump();
        }
        
        public Command StartAutoCommand { get; }
        private async Task ExecuteStartAutoCommand()
        {
            await _tankSimulationPlcHelper.StartAuto();
        }

        public MainViewModel()
        {
            _tankSimulationPlcHelper = new TankSimulationPlcHelper();

            try
            {
                _tankSimulationPlcHelper.Connect("192.168.0.89", 0, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            

            OnPlcValuesRefreshed(null, null);
            _tankSimulationPlcHelper.ValuesRefreshed += OnPlcValuesRefreshed;

            StartPumpCommand = new Command(async () => await ExecuteStartPumpCommand());
            StopPumpCommand = new Command(async () => await ExecuteStopPumpCommand());
            StartAutoCommand = new Command(async () => await ExecuteStartAutoCommand());
        }

        public void SetPumpsSpeed(short value)
        {
            _tankSimulationPlcHelper.SetPumpsSpeed(value);
        }

        public void SetFlowSpeed(short value)
        {
            _tankSimulationPlcHelper.SetFlowSpeed(value);
        }

        private void OnPlcValuesRefreshed(object sender, EventArgs e)
        {
            TankLevel = _tankSimulationPlcHelper.TankLevel;
            PumpsSpeed = _tankSimulationPlcHelper.PumpsSpeed;
            FlowSpeed = _tankSimulationPlcHelper.FlowSpeed;
        }
    }
}