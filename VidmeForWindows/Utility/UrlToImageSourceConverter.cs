using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;

namespace VidmeForWindows.Utility
{
    class UrlToImageSourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string url = value as string; 
            string format_param = parameter as string;

            if (!string.IsNullOrWhiteSpace(format_param))
            {
                switch (format_param) {
                    case "videoposterfilter":
                        Debug.WriteLine(url);
                        break;
                }
            }

            if (string.IsNullOrWhiteSpace(url))
            {
                return new BitmapImage();
            } else 
                return new BitmapImage(new Uri(url));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
