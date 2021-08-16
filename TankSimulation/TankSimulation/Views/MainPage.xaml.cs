using System;
using System.ComponentModel;
using TankSimulation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TankSimulation.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Slider slider = sender as Slider;
            MainViewModel mainViewModel = (MainViewModel)BindingContext;
            mainViewModel.SetPumps(Convert.ToInt16(slider.Value));
        }
    }
}