using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VidmeForWindows.Utility
{
    class DoubleToTimeStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string double_string = value as string;
            string result_str = "";
            Double result = 0;

            Double.TryParse(double_string, out result);

            Double minutes = Math.Floor(result / 60);
            Double seconds = Math.Floor(result - (minutes*60));
            Double hours = Math.Floor(minutes / 60);

            if(hours != 0.0)
            {
                minutes = Math.Floor(minutes - hours * 60);
                result_str += hours.ToString();
                result_str += ":";
            }

            result_str += minutes.ToString("#0");
            result_str += ":";
            result_str += seconds.ToString("00");

            return result_str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
