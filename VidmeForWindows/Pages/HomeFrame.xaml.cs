using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using VidmeForWindows.Utility;
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

    public class HomeFrameParams
    {
        public string title;
        public IncrementalLoadingVideoList videos;
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomeFrame : Page, RefreshableFrameInterface
    {
        private event Action onLoaded;
        Button SelectMultipleButton;
        public string title;
        public IncrementalLoadingVideoList videos;

        /*~HomeFrame()
        {
            SelectMultipleButton.Click -= MainPage_SelectMultipleButton;
            
        }*/

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            SelectMultipleButton.Click -= MainPage_SelectMultipleButton;
            MainView.ItemsSource = null;
            base.OnNavigatedFrom(e);
        }

        public HomeFrame()
        {
            SelectMultipleButton = ((Window.Current.Content as Frame).Content as MainPage).PublicSelectMultipleButton;
            SelectMultipleButton.Click += MainPage_SelectMultipleButton;

            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var input_params = e.Parameter as HomeFrameParams;

            videos = input_params.videos;
            title = input_params.title;

            MainView.ItemsSource = videos;
            base.OnNavigatedTo(e);

            if (onLoaded != null)
                onLoaded();
        }

        private void OnGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            

            if(e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            } else if(e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            } else if(e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }  else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)MainView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }

        }

        private void MainView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainView.SelectionMode == ListViewSelectionMode.Multiple) {
                if (MainView.SelectedItems.Count != 0)
                {
                    // display a play buttons
                    SelectMultipleButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                    SelectMultipleButton.Content = "\uE102";

                } else
                {
                    // display a select multiple

                    SelectMultipleButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                    SelectMultipleButton.Content = '\uE133';
                }
            }
        }

        private void MainPage_SelectMultipleButton(object sender, RoutedEventArgs e)
        {
            if (MainView.SelectionMode == ListViewSelectionMode.Single)
            {

                SelectMultipleButton.Background = new SolidColorBrush((Application.Current.Resources["ApplicationTheme_DarkForeground"] as SolidColorBrush).Color);
                MainView.IsItemClickEnabled = false;
                MainView.SelectionMode = ListViewSelectionMode.Multiple;
                
            } else
            {
                if (MainView.SelectedItems.Count != 0) {

                    // Play the selected videos

                } else
                {
                    MainView.SelectionMode = ListViewSelectionMode.Single;
                    MainView.IsItemClickEnabled = true;
                    SelectMultipleButton.Background = new SolidColorBrush((Application.Current.Resources["ApplicationTheme_DarkMidground"] as SolidColorBrush).Color);
                }
            }
            
        }

        private void VideoClicked_Handler(object sender, ItemClickEventArgs e)
        {
            Debug.WriteLine("Video Clicked!");
            MainPage page = ((Window.Current.Content as Frame).Content as MainPage);
            page.PublicMainFrame.Navigate(typeof(VideoFrame), new VideoFrameParams()
            {
                httpclient = page.httpclient,
                http_client_semaphore = page.http_client_semaphore,
                videos = new List<Models.Videos.Video>() { e.ClickedItem as Models.Videos.Video }
            });
        }
        public object getPageParameter()
        {
            return new HomeFrameParams()
            {
                videos = videos,
                title = title
            };
        }

    public bool multipleSupported()
    {
            return true;
    }

    public bool isRefreshable()
    {
            return true;
    }

    public string getTitleText()
    {
            if (title == null)
                return "";
            else
                return title;
    }

        public void loadedAction(Action loaded)
        {
            onLoaded += loaded;
        }
    }
}
