using System;
using System.Globalization;
using Xamarin.Forms;

namespace TankSimulation
{
    class BooleanToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? state = value as bool?;
            if (state == false)
                return Brush.Red;

            if (state == true)
                return Brush.LimeGreen;

            return Brush.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
