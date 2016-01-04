using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.Converters
{
    /// <summary>
    /// Accepts a bool value and a parameter in the format 000000:000000 
    /// (eg. #ff0000:#00ff00, for false = red and true = green), where the 
    /// first hex value represents the false colour, and the second hex 
    /// value represents the true value. Throws an exception if the 
    /// parameter is missing, or if the parameter does not contain a colon. 
    /// 
    /// Hex values may be any that can be used in Color.FromHex(...),
    /// eg. #123:##FF112233 (where the latter includes Alpha channel).
    /// </summary>
    public class BooleanToColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(parameter == null || !parameter.ToString().Contains(":"))
            {
                throw new Exception("IValueConverter Parameter should be in format #000000:#000000 (eg. #ff0000:#00ff00, for false = red and true = green) [BooleanToColourConverter in Donky.Messaging.Rich.Inbox.XamarinForms.Converters]");
            }

            var colours = parameter.ToString().Split(':');

            Color response = Color.Transparent;

            switch((bool)value)
            {
                case true:
                    response = Color.FromHex(colours[1]);
                    break;

                case false:
                    response = Color.FromHex(colours[0]);
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
