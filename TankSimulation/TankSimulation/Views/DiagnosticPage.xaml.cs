using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankSimulation.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TankSimulation.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiagnosticPage : ContentPage
    {
        public DiagnosticPage()
        {
            InitializeComponent();
            var diagnosticViewModel = new DiagnosticViewModel(this);
            BindingContext = diagnosticViewModel;
        }
    }
}