using EsperanceClient.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EsperanceClient.Converters
{
    public class DeviceOrientationToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool response = false;
            var deviceOrientation = (DeviceOrientation)value;
            var targetOrientation = parameter.ToString();

            if (deviceOrientation.ToString().Equals(targetOrientation))
            {
                response = true;
            }

            return response;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
