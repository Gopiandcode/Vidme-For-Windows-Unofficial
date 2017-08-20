using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using VidmeForWindows.Utility;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VidmeForWindows.Pages
{

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public enum UsersState
    {
        Following,
        Featured,
        Search
    }

    public class UserFrameParams
    {
        public string id;
        public HttpClient httpClient;
        public SemaphoreSlim http_client_semaphore;
    }

    public sealed partial class UsersFrame : Page
    {
        private string id;
        private HttpClient httpClient;
        private SemaphoreSlim http_client_semaphore;

        UsersState user_state;
        UsersState last_state;

        public UsersFrame()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UserFrameParams sent = e.Parameter as UserFrameParams;


            id = sent.id;
            httpClient = sent.httpClient;
            http_client_semaphore = sent.http_client_semaphore;
            

            if(id is null)
            {
                UsersFromFollowing.IsEnabled = false;
                UsersFromFeatured.IsChecked = true;
                SetState(UsersState.Featured, null, true );


            } else
            {
                UsersFromFollowing.IsChecked = true;
                SetState(UsersState.Following, null, true);
            }

            base.OnNavigatedTo(e);
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

        private void UserClicked_Handler(object sender, ItemClickEventArgs e)
        {

        }

        

        void SetState(UsersState state, string url)
        {
            SetState(state, url, false);
        }


        void SetState(UsersState state, string url, Boolean ignoreState)
        {

            if (state == user_state && !ignoreState) return;

            switch (state)
            {
                case UsersState.Featured:
                    UserSourceFlyoutText.Text = "Featured";
                    SearchTextBox.Text = "";
                    // Create a Following URL
                    string featured_url = Config.VidmeUrlClass.FeaturedUserURL;
                    MainView.ItemsSource = new IncrementalLoadingUserList(featured_url + "?", http_client_semaphore, httpClient);


                    break;
                case UsersState.Following:
                    UserSourceFlyoutText.Text = "Following";
                    SearchTextBox.Text = "";
                    // Create  a Following List
                    string following_user_url = Config.VidmeUrlClass.FollowingUserURL(id);
                    MainView.ItemsSource = new IncrementalLoadingUserList(following_user_url + "?", http_client_semaphore, httpClient);

                    break;
                case UsersState.Search:

                    // create users list
                    if(user_state != UsersState.Search)
                        last_state = user_state;
                    if(url != null)
                        MainView.ItemsSource = new IncrementalLoadingUserList(url + "&", http_client_semaphore, httpClient);

                    break;
            }

            user_state = state;

        }


        private void updateSearch()
        {
            string search = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(search) && user_state == UsersState.Search)
            {
                SetState(last_state, null, true);
            }
            else if(!string.IsNullOrEmpty(search))
            {
                string escaped_string = Uri.EscapeDataString(search);
                string search_url = Config.VidmeUrlClass.SearchUserURL(escaped_string);

                SetState(UsersState.Search, search_url, true);
            }
        }


        private void VideoSource_Changed(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;

            if(button.Name == "UsersFromFollowing" && id != null)
            {
                SetState(UsersState.Following, null);

            } else
            {
                SetState(UsersState.Featured, null);
            }
        }

        private void SearchTextBoxTextChanged(TextBox sender, TextBoxTextChangingEventArgs args)
        {
            updateSearch();

        }

        private void ClearTextHandler(object sender, RoutedEventArgs e)
        {
            SearchTextBox.Text = "";
            updateSearch();
        }
    }
}
