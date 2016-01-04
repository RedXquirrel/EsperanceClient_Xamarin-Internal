using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.Converters
{
    public class NewMailDateTimeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTime input;

            if(value != null)
            {
                input = (DateTime)value;
            }

            string para = parameter.ToString();

            bool response = false;

            if(para.Equals("null"))
            {
                if(value == null)
                {
                    response = true;
                }
                else
                {
                    response = false;
                }
            }

            if(para.Equals("notnull"))
            {
                if (value == null)
                {
                    response = false;
                }
                else
                {
                    response = true;
                }
            }

            return response;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
