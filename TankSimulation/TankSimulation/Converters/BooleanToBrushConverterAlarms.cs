using System;
using System.Globalization;
using Xamarin.Forms;

namespace TankSimulation
{
    class BooleanToBrushConverterAlarms : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? state = value as bool?;
            if (state == false)
                return Brush.White;

            if (state == true)
                return Brush.Red;

            return Brush.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
