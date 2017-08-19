using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VidmeForWindows.Utility
{
    class StringAppenderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            String appending = parameter as String;
            String result;

            Debug.WriteLine(appending);

            if (appending != null && appending.Equals("views"))
            {
                Int32 val = (Int32)value;
                result = val.ToString();
            } else
                result = (string)value;

            Debug.WriteLine(result);


            if (appending != null)
            {
                result += " ";
                result += appending;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
