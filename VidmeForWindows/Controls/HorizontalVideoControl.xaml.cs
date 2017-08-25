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
    public sealed partial class HorizontalVideoControl : UserControl
    {
        public static DependencyProperty VideoTitleProperty = DependencyProperty.Register("VideoTitle", typeof(string), typeof(HorizontalVideoControl), null);
        public static DependencyProperty VideoViewCountProperty = DependencyProperty.Register("VideoViewCount", typeof(string), typeof(HorizontalVideoControl), null);
        public static DependencyProperty VideoLengthProperty = DependencyProperty.Register("VideoLength", typeof(string), typeof(HorizontalVideoControl), null);
        public static DependencyProperty VideoThumbnailProperty = DependencyProperty.Register("VideoThumbnail", typeof(ImageSource), typeof(HorizontalVideoControl), null);
        public static DependencyProperty VideoPosterProperty = DependencyProperty.Register("VideoPoster", typeof(ImageSource), typeof(HorizontalVideoControl), null);
        public static DependencyProperty VideoPosterNameProperty = DependencyProperty.Register("VideoPosterName", typeof(string), typeof(HorizontalVideoControl), null);



        public string VideoTitle
        {
            get { return (string)GetValue(VideoTitleProperty); }
            set { SetValue(VideoTitleProperty, value); }
        }

        public string VideoViewCount
        {
            get { return (string)GetValue(VideoViewCountProperty); }
            set { SetValue(VideoViewCountProperty, value); }
        }

        public string VideoLength
        {
            get { return (string)GetValue(VideoLengthProperty); }
            set { SetValue(VideoLengthProperty, value); }
        }

        public ImageSource VideoThumbnail
        {
            get { return (ImageSource)GetValue(VideoThumbnailProperty); }
            set { SetValue(VideoThumbnailProperty, value); }
        }

        public ImageSource VideoPoster
        {
            get { return (ImageSource)GetValue(VideoPosterProperty); }
            set { SetValue(VideoPosterProperty, value); }
        }

        public string VideoPosterName
        {
            get { return (string)GetValue(VideoPosterNameProperty); }
            set { SetValue(VideoPosterNameProperty, value); }
        }


        public HorizontalVideoControl()
        {
            this.InitializeComponent();
        }
    }
}
