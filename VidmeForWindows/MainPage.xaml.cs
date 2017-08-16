using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using VidmeForWindows.Config;
using VidmeForWindows.Dialogs;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Security.Authentication.Web;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VidmeForWindows
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        HttpClient httpclient;

        public MainPage()
        {
            httpclient = new HttpClient();

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", VidmeAuthentificationClass.Application_Key, VidmeAuthentificationClass.Application_Secret)));


            httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
            this.InitializeComponent();

            //PC customization
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
               
                ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = (Application.Current.Resources["ApplicationTheme_DarkAccent"] as SolidColorBrush).Color;
                    titleBar.ButtonForegroundColor = (Application.Current.Resources["ApplicationTheme_Highlight"] as SolidColorBrush).Color;
                    titleBar.BackgroundColor = (Application.Current.Resources["ApplicationTheme_DarkAccent"] as SolidColorBrush).Color;
                    titleBar.ForegroundColor = (Application.Current.Resources["ApplicationTheme_Highlight"] as SolidColorBrush).Color;


                    //titleBar.ButtonBackgroundColor = Colors.Transparent;
                    CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                    coreTitleBar.ExtendViewIntoTitleBar = false;

                }
            }

        }

        private void HamburgerButtonClick(object sender, RoutedEventArgs e)
        {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FeedButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void WatchLaterButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TeamPicksButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void HotTodayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FreshUploadsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CreatorsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FollowingButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FeaturedButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            /*var dialog = new LoginDialog();

            await dialog.ShowAsync();


            if(dialog.Result == LoginDialogState.Failed)
            {
                var error_dialog = new ContentDialog() {
                    Title = "Login error",
                    Content = "Unfortunately we could not log you in.",
                    PrimaryButtonText = "Ok",
                    Background = Application.Current.Resources["ApplicationTheme_DarkForeground"] as Brush,
                    RequestedTheme =  ElementTheme.Dark
                };
                

                await error_dialog.ShowAsync();
            } else if(dialog.Result == LoginDialogState.Login)
            {
                
            } */

            //http://gopiandcode.com/VidmeForWindows
            /* &redirect_uri=ms-app%3A%2F%2Fs-1-15-2-2550072175-1356320857-1890753780-960594463-2124538076-1866243324-1867296574%2F */
            Debug.WriteLine(WebAuthenticationBroker.GetCurrentApplicationCallbackUri());
            string url = "https://vid.me/oauth/authorize?client_id=9ciI8pH3O1elCy2okUWLtsnaMrimRh7T&redirect_uri=ms-app%3A%2F%2Fs-1-15-2-2550072175-1356320857-1890753780-960594463-2124538076-1866243324-1867296574%2F&scopes=votes%20comments%20channels%20basic&response_type=token";
            string startURL = "https://vid.me/oauth/authorize?client_id=" + VidmeAuthentificationClass.Application_ClientID + "&redirect_uri=http%3A%2F%2Fgopiandcode.com%2FVidmeForWindows&scopes=votes%20comments%20channels%20basic&response_type=code";
            string endURL = "http://gopiandcode.com/VidmeForWindows";

            Uri startURI = new Uri(startURL);
            Uri endURI = new Uri(endURL);

            try
            {
                var webAuthenticationResult =
                    await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                    startURI,
                    endURI);

                Debug.WriteLine(webAuthenticationResult.ResponseData.ToString());

                string auth_code = webAuthenticationResult.ResponseData.ToString();
                auth_code = auth_code.Substring(auth_code.LastIndexOf('=') + 1);

                string auth_token_uri = "https://api.vid.me/oauth/token";
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(auth_token_uri),
                    Method = HttpMethod.Post
                };


                var body = new List<KeyValuePair<string, string>>();
                body.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                body.Add(new KeyValuePair<string, string>("code", auth_code));

                var content = new FormUrlEncodedContent(body);


                request.Content = content;

                HttpResponseMessage msg = await httpclient.SendAsync(request);

                var responsecontent = await msg.Content.ReadAsStringAsync();

                JsonConvert.DeserializeObject<VidmeForWindows.Models.AuthenticationResponse.Rootobject>(responsecontent);

                Debug.WriteLine(responsecontent);


            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            


        }

        private void SearchButtonClick(object sender, RoutedEventArgs e)
        {

        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
