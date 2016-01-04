using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Donky.Messaging.Rich.Inbox.XamarinForms.Converters
{
    public class ItemDateTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            /*
                [14:24:33] Chris Wunsch: Less than 5 minutes = Just now
                [14:24:44 | Edited 14:24:45] Chris Wunsch: More than 2 minutes but less than 1 hour = X min ago
                [14:24:56] Chris Wunsch: more than 1 hour but still on the same day = X hours ago
                [14:25:08] Chris Wunsch: more than 1 hour but the previous day = yesterday
                [14:25:16] Chris Wunsch: more than 24 hours (i.e. yesterday) = yesterday
                [14:25:34] Chris Wunsch: more than 24 hours but less than 1 week ago = Name of day i.e. Tuesday
                [14:25:43] Chris Wunsch: more than 1 week ago = 22/09/2015             * */

            string response = string.Empty;

            var isResponse = false;

            var now = DateTime.UtcNow;
            var sent = (DateTime)value;

            int totalMinutesSinceSent = (int)Math.Ceiling((now - sent).TotalMinutes);
            int totalHoursSinceSent = (int)Math.Ceiling((now - sent).TotalHours);

            bool wasSentToday = ((sent.Date == now.Date) &&(sent.Month == now.Month) && (sent.Year == now.Year));
            bool wasSentYesterday = sent == DateTime.Now.AddDays(-1);

            bool wasSentGreaterOrEqualTo1HourAgo = totalMinutesSinceSent >= 60;
            bool wasSentGreaterThan24HoursAgo = (totalHoursSinceSent > 24);
            bool wasSentGreaterThan48HoursAgo = (totalHoursSinceSent > 48);
            bool wasSentWithinLast7Days = DateTime.Now.Subtract(sent).Days <= 7;
            bool wasSentGreaterThanSevenDaysAgo = DateTime.Now.Subtract(sent).Days > 7;

            // Less than 5 minutes = Just now
            if ((totalMinutesSinceSent < 2) && !isResponse)
            {
                response = "Just now";
                isResponse = true;
            }
            // More than 5 minutes but less than 1 hour = X min ago
            else if (((totalMinutesSinceSent >= 2) && (totalMinutesSinceSent < 60)) && !isResponse)
            {
                response = string.Format("{0} min ago", totalMinutesSinceSent.ToString());
                isResponse = true;
            }
            // more than 1 hour but still on the same day = X hours ago
            else if ((wasSentGreaterOrEqualTo1HourAgo && wasSentToday) && !isResponse)
            {
                response = string.Format("{0} hours ago", totalHoursSinceSent.ToString());
                isResponse = true;
            }
            // more than 1 hour but the previous day = yesterday    
            else if ((wasSentGreaterOrEqualTo1HourAgo && wasSentYesterday) && !isResponse)
            {
                response = "yesterday";
                isResponse = true;
            }
            // more than 24 hours ago but less than 48 hours
            else if ((wasSentGreaterThan24HoursAgo && !wasSentGreaterThan48HoursAgo) && !isResponse)
            {
                response = "yesterday";
                isResponse = true;
            }
            // more than 48 hours but less than 1 week ago = Name of day i.e. Tuesday
            else if ((wasSentGreaterThan48HoursAgo && wasSentWithinLast7Days) && !isResponse)
            {
                response = sent.DayOfWeek.ToString();
                isResponse = true;
            }
            // more than 1 week ago = 22/09/2015
            else if ((wasSentWithinLast7Days) && !isResponse)
            {
                response = string.Format("{0:dd/MM/yy}", sent);
                isResponse = true;
            }
            else
            {
                response = string.Format("{0:dd/MM/yy}", sent);
                isResponse = true;
            }


            
            return response;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
