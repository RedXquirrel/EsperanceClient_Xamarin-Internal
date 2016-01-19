using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EsperanceClient.Converters
{
    public class TargetIdiomToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool response = false;

            var deviceIdiom = (TargetIdiom)value;
            var targetIdiom = parameter.ToString();

            if(deviceIdiom.ToString().Equals(targetIdiom))
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
