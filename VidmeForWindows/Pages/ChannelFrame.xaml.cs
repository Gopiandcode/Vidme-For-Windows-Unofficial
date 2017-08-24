using System;
using System.Collections.Generic;
using System.Diagnostics;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VidmeForWindows.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChannelFrame : Page
    {
        public ChannelFrame()
        {
            this.InitializeComponent();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<Models.Channels.Channel> channels = e.Parameter as List<Models.Channels.Channel>;
            MainView.ItemsSource = channels;
            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            MainView.ItemsSource = null;
            base.OnNavigatedFrom(e);
        }

        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {


            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }

        }


        public void MainView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ChannelItemClick_Handler(object sender, ItemClickEventArgs e)
        {
            Models.Channels.Channel channel = e.ClickedItem as Models.Channels.Channel;

            string channel_video_URL = "https://api.vid.me/channel/" + channel.channel_id.ToString() +  "/hot";
            ((Window.Current.Content as Frame).Content as MainPage).displayChannelVideos(channel_video_URL, channel.title);


        }
    }

}
