using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace VidmeForWindows.Controls
{
    public sealed partial class ChannelControl : UserControl
    {
        public static DependencyProperty ChannelThumbnailProperty = DependencyProperty.Register("ChannelThumbnailImage", typeof(ImageSource), typeof(ChannelControl), null);
        public static DependencyProperty ChannelPosterProperty = DependencyProperty.Register("ChannelPosterImage", typeof(ImageSource), typeof(ChannelControl), null);
        public static DependencyProperty ChannelTitleProperty = DependencyProperty.Register("ChannelTitleString", typeof(string), typeof(ChannelControl), null);
        public static DependencyProperty ChannelVideoCountProperty = DependencyProperty.Register("ChannelVideoCountString", typeof(string), typeof(ChannelControl), null);

        public ImageSource ChannelThumbnailImage
        {
            get { return (ImageSource)GetValue(ChannelThumbnailProperty); }
            set { SetValue(ChannelThumbnailProperty, value);  }
        }
        public ImageSource ChannelPosterImage
        {
            get { return (ImageSource)GetValue(ChannelPosterProperty); }
            set { SetValue(ChannelPosterProperty, value);  }
        }
        public string ChannelTitleString
        {
            get { return (string)GetValue(ChannelTitleProperty); }
            set { SetValue(ChannelTitleProperty, value);  }
        }
        public string ChannelVideoCountString
        {
            get { return (string)GetValue(ChannelVideoCountProperty);}
            set { SetValue(ChannelVideoCountProperty, value);  }
        }

        public ChannelControl()
        {
            this.InitializeComponent();
        }
    }
}
