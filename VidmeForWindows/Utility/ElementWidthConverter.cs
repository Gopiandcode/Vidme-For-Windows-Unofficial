using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace VidmeForWindows.Utility
{
    class ElementWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Debug.WriteLine("COnvertin whooowhowooo");
            Double width = (double) value;
            String fmt_param = parameter as String;
            if(fmt_param != null)
            {
                Debug.WriteLine(fmt_param);
                // expects string in the format OPERATOR,NUMBER
                string number = fmt_param.Substring(1);
                double result;
                if(Double.TryParse(number, out result))
                {
                    switch(fmt_param[0])
                    {
                        case '/':
                            width /= result;
                            break;
                        case '*':
                            width *= result;
                            break;
                    }

                }
            }


            return width;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
