using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.Converters
{
    public class BooleanToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter == null || !parameter.ToString().Contains(":"))
            {
                throw new Exception("IValueConverter Parameter should be in format 0:50 (eg. 0:50, for false = 0 and true = 100) [BooleanToDoubleConverter in Donky.Messaging.Rich.Inbox.XamarinForms.Converters]");
            }

            var values = parameter.ToString().Split(':');

            GridLength response = new GridLength(0);

            switch ((bool)value)
            {
                case true:
                    response = new GridLength(System.Convert.ToDouble(values[1]));
                    break;

                case false:
                    response = new GridLength(System.Convert.ToDouble(values[0]));
                    break;
            }

            return response;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
