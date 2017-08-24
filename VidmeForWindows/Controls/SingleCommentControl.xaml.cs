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
    public sealed partial class SingleCommentControl : UserControl
    {
        public static DependencyProperty UserAvatarProperty = DependencyProperty.Register("UserAvatar", typeof(ImageSource), typeof(SingleCommentControl), null);
        public static DependencyProperty UserNameProperty = DependencyProperty.Register("UserName", typeof(string), typeof(SingleCommentControl), null);
        public static DependencyProperty UserPostDateProperty = DependencyProperty.Register("UserPostDate", typeof(string), typeof(SingleCommentControl), null);


        public static DependencyProperty VideoNameProperty = DependencyProperty.Register("VideoName", typeof(string), typeof(SingleCommentControl), null);
        public static DependencyProperty CommentProperty = DependencyProperty.Register("Comment", typeof(string), typeof(SingleCommentControl), null);


        public ImageSource UserAvater
        {
            get { return (ImageSource)GetValue(UserAvatarProperty); }
            set { SetValue(UserAvatarProperty, value); }
        }


        public  string UserName
        {
            get { return (string)GetValue(UserNameProperty); }
            set { SetValue(UserNameProperty, value); }
        }


        public string UserPostDate
        {
            get { return (string)GetValue(UserPostDateProperty); }
            set { SetValue(UserPostDateProperty, value); }
        }

        public string VideoName
        {
            get { return (string)GetValue(VideoNameProperty); }
            set { SetValue(VideoNameProperty, value); }
        }

        public string Comment
        {
            get { return (string)GetValue(CommentProperty); }
            set { SetValue(CommentProperty, value); }
        }






        public SingleCommentControl()
        {
            this.InitializeComponent();
        }
    }
}
