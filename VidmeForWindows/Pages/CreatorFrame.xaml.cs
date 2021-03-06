﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
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

    public sealed partial class CreatorFrame : Page, RefreshableFrameInterface
    {
        private Models.User.User user;
        private HttpClient httpClient;
        private SemaphoreSlim http_client_semaphore;
        Button SelectMultipleButton;
        string id;
        Boolean isfollowing;
        public event Action onLoaded;

        public CreatorFrame()
        {
            SelectMultipleButton = ((Window.Current.Content as Frame).Content as MainPage).PublicSelectMultipleButton;
            SelectMultipleButton.Click += this.MainPage_SelectMultipleButton;

            this.InitializeComponent();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            base.OnNavigatedFrom(e);
            SelectMultipleButton.Click -= this.MainPage_SelectMultipleButton;
            GridViewUserVideos.ItemsSource = null;
            GridViewUpvotedVideos.ItemsSource = null;

            FollowingView.ItemsSource = null;
            FollowersView.ItemsSource = null;

            AlbumView.ItemsSource = null;
            CommentsView.ItemsSource = null;

            CoverImage.Source = null;

            UserIdImage.ImageSource = null;

        }

        private void MainPage_SelectMultipleButton(object sender, RoutedEventArgs e)
        {
            string selection_name = ((MainPivot.SelectedItem as PivotItem).Header as TextBlock).Text;

            GridView MainView = null;

            switch(selection_name)
            {
                case "Home":
                    MainView = GridViewUserVideos;
                    break;
                case "Upvoted":
                    MainView = GridViewUpvotedVideos;
                    break;
                case "Albums":
                    return;
                case "Following":
                    return;
                case "Comments":
                    return;
                case "Followers":
                    return;
            }

            if (MainView.SelectionMode == ListViewSelectionMode.Single)
            {

                SelectMultipleButton.Background = new SolidColorBrush((Application.Current.Resources["ApplicationTheme_DarkForeground"] as SolidColorBrush).Color);
                MainView.IsItemClickEnabled = false;
                MainView.SelectionMode = ListViewSelectionMode.Multiple;

            }
            else
            {
                if (MainView.SelectedItems.Count != 0)
                {

                    // Play the selected videos

                }
                else
                {
                    MainView.SelectionMode = ListViewSelectionMode.Single;
                    MainView.IsItemClickEnabled = true;
                    SelectMultipleButton.Background = new SolidColorBrush((Application.Current.Resources["ApplicationTheme_DarkMidground"] as SolidColorBrush).Color);
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            CreatorFrameParams input = e.Parameter as CreatorFrameParams;

            httpClient = input.httpClient;
            http_client_semaphore = input.http_client_semaphore;
            user = input.user;
            id = input.id;

            if(id == null)
            {
                FollowButton.IsEnabled = false;
            } else
            {
                if (user.is_following) FollowButton.Content = "Unfollow";
                isfollowing = user.is_following;
            }

            CoverImage.Source = new BitmapImage(new Uri(user.cover_url));
            
            UserIdImage.ImageSource = new BitmapImage(new Uri(user.avatar_url));

            VidmeUser.Text = user.displayname == null ? "" : user.displayname;

            VidmeViewCount.Text = user.follower_count + " followers";

            UserText.Text = user.bio == null ? "" : user.bio;

            GridViewUserVideos.ItemsSource = new IncrementalLoadingVideoList(Config.VidmeUrlClass.UserVideoURL(user.user_id), http_client_semaphore, httpClient);

            GridViewUpvotedVideos.ItemsSource = new IncrementalLoadingVideoList(Config.VidmeUrlClass.LikedVideosUserURL(user.user_id), http_client_semaphore, httpClient);

            FollowingView.ItemsSource = new IncrementalLoadingUserList(Config.VidmeUrlClass.FollowingUserURL(user.user_id) + "?", http_client_semaphore, httpClient);

            FollowersView.ItemsSource = new IncrementalLoadingUserList(Config.VidmeUrlClass.FollowersUserURL(user.user_id) + "?", http_client_semaphore, httpClient);

            AlbumView.ItemsSource = new IncrementalLoadingAlbumList(Config.VidmeUrlClass.UserAlbumsURL(user.user_id), http_client_semaphore, httpClient);
            CommentsView.ItemsSource = new IncrementalLoadingCommentList(Config.VidmeUrlClass.UserCommentsURL(user.user_id), http_client_semaphore, httpClient);

            base.OnNavigatedTo(e);

            if (onLoaded != null)
                onLoaded();
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
            GridView selection = sender as GridView;
            if (selection.SelectionMode == ListViewSelectionMode.Multiple)
            {
                if (selection.SelectedItems.Count != 0)
                {
                    // display a play buttons
                    SelectMultipleButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                    SelectMultipleButton.Content = "\uE102";

                }
                else
                {
                    // display a select multiple

                    SelectMultipleButton.FontFamily = new FontFamily("Segoe MDL2 Assets");
                    SelectMultipleButton.Content = '\uE133';
                }
            }
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

            Models.Album.Album alb = e.ClickedItem as Models.Album.Album;
            ((Window.Current.Content as Frame).Content as MainPage).PublicMainFrame.Navigate(typeof(HomeFrame),

                new HomeFrameParams() {

                    videos = new IncrementalLoadingVideoList(
                Config.VidmeUrlClass.AlbumVideosURL(alb.album_id),
                http_client_semaphore,
                httpClient,
                new IncrementalLoadingVideoList.retrieve_request_message( (string url) => {
                    return new HttpRequestMessage()
                    {
                        RequestUri = new Uri(url),
                        Method = HttpMethod.Get
                    }; ; })),
                    title = alb.title


                }
                    );
        
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

            ((Window.Current.Content as Frame).Content as MainPage).CreatorNavigate(typeof(CreatorFrame), item);
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

        private void PivotSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PivotItem pivot = (PivotItem)(sender as Pivot).SelectedItem;




            GridViewUpvotedVideos.SelectionMode = ListViewSelectionMode.Single;
            GridViewUserVideos.SelectionMode = ListViewSelectionMode.Single;


            GridViewUpvotedVideos.IsItemClickEnabled = true;
            GridViewUserVideos.IsItemClickEnabled = true;

            SelectMultipleButton.Background = new SolidColorBrush((Application.Current.Resources["ApplicationTheme_DarkMidground"] as SolidColorBrush).Color);
            SelectMultipleButton.Content = '\uE133';


            switch ((pivot.Header as TextBlock).Text)
            {
                case "Home":
                    SelectMultipleButton.Visibility = Visibility.Visible;

                    break;
                case "Upvoted":
                    SelectMultipleButton.Visibility = Visibility.Visible;


                    break;
                case "Albums":
                    SelectMultipleButton.Visibility = Visibility.Collapsed;




                    break;
                case "Following":
                    SelectMultipleButton.Visibility = Visibility.Collapsed;


                    break;
                case "Comments":
                    SelectMultipleButton.Visibility = Visibility.Collapsed;





                    break;
                case "Followers":
                    SelectMultipleButton.Visibility = Visibility.Collapsed;





                    break;
            }
        }

        private void OnCommentsViewSizeChanged(object sender, SizeChangedEventArgs e)
        {
            //FollowersView
            if (e.NewSize.Width > 1000)
            {
                Debug.WriteLine(">1000 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)CommentsView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / columns) - 5;

            }
            else if (e.NewSize.Width > 800)
            {
                Debug.WriteLine(">800 " + ActualWidth);
                var columns = Math.Ceiling(ActualWidth / 400);
                ((ItemsWrapGrid)CommentsView.ItemsPanelRoot).ItemWidth = (e.NewSize.Width / 3) - 5;

            }
            else if (e.NewSize.Width > 650)
            {
                Debug.WriteLine(">650 " + ActualWidth);
                //var columns = Math.Ceiling(ActualWidth / 500);
                ((ItemsWrapGrid)CommentsView.ItemsPanelRoot).ItemWidth = Math.Floor(e.NewSize.Width / 2) - 5;

            }
            else
            {
                Debug.WriteLine("else " + ActualWidth);
                ((ItemsWrapGrid)CommentsView.ItemsPanelRoot).ItemWidth = ActualWidth;
            }
        }

        private void CommentClicked_Handler(object sender, ItemClickEventArgs e)
        {

        }
        Task followbuttontask;

        private async Task FollowChange() {
            if (isfollowing)
            {
                HttpRequestMessage message = new HttpRequestMessage()
                {
                    RequestUri = new Uri(Config.VidmeUrlClass.UserUnfollowURL(user.user_id)),
                    Method = HttpMethod.Post
                };

                await http_client_semaphore.WaitAsync();
                HttpResponseMessage response = await httpClient.SendAsync(message);
                http_client_semaphore.Release();

                string response_string = await response.Content.ReadAsStringAsync();

                var status = new { status = true };

                var data = JsonConvert.DeserializeAnonymousType(response_string, status);


                if (data.status)
                {
                    isfollowing = false;
                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        FollowButton.Content = "Follow";
                    });
                }


            }
            else
            {

                HttpRequestMessage message = new HttpRequestMessage()
                {
                    RequestUri = new Uri(Config.VidmeUrlClass.UserFollowURL(user.user_id)),
                    Method = HttpMethod.Post
                };

                await http_client_semaphore.WaitAsync();
                HttpResponseMessage response = await httpClient.SendAsync(message);
                http_client_semaphore.Release();

                string response_string = await response.Content.ReadAsStringAsync();

                var status = new { status = true };

                var data = JsonConvert.DeserializeAnonymousType(response_string, status);


                if (data.status)
                {
                    isfollowing = true;

                    await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        FollowButton.Content = "UnFollow";
                    });


                }
            }

            }

        private void FollowButtonClick(object sender, RoutedEventArgs e)
        {
            if (id != null)
            {
                if (followbuttontask != null)
                    followbuttontask = followbuttontask.ContinueWith((t) => { return FollowChange(); });
                else
                    followbuttontask = FollowChange();

            }
        }

        public object getPageParameter()
        {
            return new CreatorFrameParams()
            {
                httpClient = httpClient,
                http_client_semaphore = http_client_semaphore,
                id = id,
                user = user
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
            if (user.displayname != null)
                return user.displayname;
            else return "";
        }

        public void loadedAction(Action loaded)
        {
            onLoaded += loaded;
        }
    }
}
