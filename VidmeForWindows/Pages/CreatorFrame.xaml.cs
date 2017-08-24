using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using VidmeForWindows.Utility;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VidmeForWindows.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public class CreatorFrameParams
    {
        public HttpClient httpClient;
        public SemaphoreSlim http_client_semaphore;
        public Models.User.User user;
        public string id;
    }

    public sealed partial class CreatorFrame : Page
    {
        private Models.User.User user;
        private HttpClient httpClient;
        private SemaphoreSlim http_client_semaphore;
        Button SelectMultipleButton;
        string id;


        public CreatorFrame()
        {
            SelectMultipleButton = ((Window.Current.Content as Frame).Content as MainPage).PublicSelectMultipleButton;
            SelectMultipleButton.Click += MainPage_SelectMultipleButton;

            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);
            SelectMultipleButton.Click -= MainPage_SelectMultipleButton;
        }

        private void MainPage_SelectMultipleButton(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CreatorFrameParams input = e.Parameter as CreatorFrameParams;

            httpClient = input.httpClient;
            http_client_semaphore = input.http_client_semaphore;
            user = input.user;
            id = input.id;

            CoverImage.Source = new BitmapImage(new Uri(user.cover_url));
            UserIdImage.ImageSource = new BitmapImage(new Uri(user.avatar_url));
            GridViewUserVideos.ItemsSource = new IncrementalLoadingVideoList(Config.VidmeUrlClass.UserVideoURL(user.user_id), http_client_semaphore, httpClient);

            GridViewUpvotedVideos.ItemsSource = new IncrementalLoadingVideoList(Config.VidmeUrlClass.LikedVideosUserURL(user.user_id), http_client_semaphore, httpClient);

            FollowingView.ItemsSource = new IncrementalLoadingUserList(Config.VidmeUrlClass.FollowingUserURL(user.user_id) + "?", http_client_semaphore, httpClient);

            FollowersView.ItemsSource = new IncrementalLoadingUserList(Config.VidmeUrlClass.FollowersUserURL(user.user_id) + "?", http_client_semaphore, httpClient);


            base.OnNavigatedTo(e);
        }

        private void OnUserGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)GridViewUserVideos.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;
                
            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)GridViewUserVideos.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)GridViewUserVideos.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)GridViewUserVideos.ItemsPanelRoot).ItemWidth = ActualWidth;
            }

        }

        private void VideoClicked_Handler(object sender, ItemClickEventArgs e)
        {

        }

        private void MainView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void OnUpvotedGridViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)GridViewUpvotedVideos.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)GridViewUpvotedVideos.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)GridViewUpvotedVideos.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)GridViewUpvotedVideos.ItemsPanelRoot).ItemWidth = ActualWidth;
            }
        }

        private void OnGridViewChannelSizeChanged(object sender, SizeChangedEventArgs e)
        {
            
            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)AlbumView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)AlbumView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)AlbumView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)AlbumView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }
        }

        private void OnFollowingViewSizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)FollowingView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)FollowingView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)FollowingView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)FollowingView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }
        }

        private void ChannelItemClick_Handler(object sender, ItemClickEventArgs e)
        {

        }

        private void UserClicked_Handler(object sender, ItemClickEventArgs e)
        {

            Models.User.User person = e.ClickedItem as Models.User.User;

            CreatorFrameParams item = new CreatorFrameParams()
            {
                id = id,
                httpClient = httpClient,
                http_client_semaphore = http_client_semaphore,
                user = person
            };

            ((Window.Current.Content as Frame).Content as MainPage).Navigate(typeof(CreatorFrame), item);
        }

        private void OnFollowerViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //FollowersView
            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)FollowersView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)FollowersView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)FollowersView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)FollowersView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }
        }
    }
}
