using System;
using System.ComponentModel;
using TankSimulation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TankSimulation.Views
{
    public partial class MainPage : ContentPage
    {
        public  MainPage()
        {
            InitializeComponent();
            var mainViewModel = new MainViewModel(this);
            BindingContext = mainViewModel;
            try
            {
                FlowSlider.Value = 2.0;
                PumpsSlider.Value = 1.0;
            }
            catch 
            {
                Application.Current.MainPage.DisplayAlert("Alert", "Brak połaczenia ze sterownikiem PLC, zresetuj aplikacje", "Ok");
            }
        }

        private void FlowSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            MainViewModel mainViewModel = (MainViewModel)BindingContext;
            mainViewModel.SetFlowSpeed(slider.Value);
        }

        private void PumpsSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            MainViewModel mainViewModel = (MainViewModel)BindingContext;
            mainViewModel.SetPumpsSpeed(slider.Value);
        }
    }
}