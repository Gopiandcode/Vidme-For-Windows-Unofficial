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
    public sealed partial class UserControls : UserControl
    {

        public static DependencyProperty UsernameProperty = DependencyProperty.Register("Username", typeof(string), typeof(UserControls), null);
        public static DependencyProperty VideoCountProperty = DependencyProperty.Register("VideoCount", typeof(string), typeof(UserControls), null);
        public static DependencyProperty FollowerCountProperty = DependencyProperty.Register("FollowerCount", typeof(string), typeof(UserControls), null);
        public static DependencyProperty UserAvatarImageProperty = DependencyProperty.Register("UserAvatarImage", typeof(ImageSource), typeof(UserControls), null);

        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public string VideoCount
        {
            get { return (string)GetValue(VideoCountProperty); }
            set { SetValue(VideoCountProperty, value); }
        }

        public string FollowerCount
        {
            get { return (string)GetValue(FollowerCountProperty); }
            set { SetValue(FollowerCountProperty, value); }
        }

        public ImageSource UserAvatarImage
        {
            get { return (ImageSource)GetValue(UserAvatarImageProperty); }
            set { SetValue(UserAvatarImageProperty, value); }
        }


        public UserControls()
        {
            this.InitializeComponent();
        }
    }
}
