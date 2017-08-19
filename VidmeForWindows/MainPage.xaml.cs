﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidmeForWindows.Config;
using VidmeForWindows.Dialogs;
using VidmeForWindows.Pages;
using VidmeForWindows.Utility;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VidmeForWindows
{


    public sealed partial class MainPage : Page
    {
        static string resourcename = "vidmeforwindows";
        public HttpClient httpclient;
        PasswordVault vault;
        Boolean loggedIn;
        Task http_client_task;
        ContentDialog dialog;
        public SemaphoreSlim http_client_semaphore = new SemaphoreSlim(1, 1);

        public ProgressRing contentLoadingRing
        {
            get { return ProgressRing; }
        }

        private PasswordCredential getCredentialsFromLocker()
        {

            if (vault == null) vault = new PasswordVault();
            PasswordCredential credential = null;
            IReadOnlyList<PasswordCredential> credentialList = null;

            try
            {
                credentialList = vault.FindAllByResource(resourcename);

            } catch(Exception)
            {}
            if(credentialList != null && credentialList.Count > 0)
            {

                if (credentialList.Count == 1)
                    credential = credentialList[0];
                else
                    throw new NotImplementedException("More than one credential in the password vault.");

            }
            
            return credential;
        }


        void configureHttpClient()
        {
            http_client_semaphore.Wait();
            httpclient = new HttpClient();
            http_client_semaphore.Release();

            //var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", VidmeAuthentificationClass.Application_Key, VidmeAuthentificationClass.Application_Secret)));
            
            //httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        }
        
        // Will attempt to login to the application, and change the respective values
        async Task tryLoginAsync()
        {
            if (vault == null) vault = new PasswordVault();
            if (httpclient == null) configureHttpClient();

            var credentials = getCredentialsFromLocker();

            if(credentials != null)
            {
                credentials.RetrievePassword();

                // password found.

                var authentication_token = credentials.Password;


                // Apply authentication token to httpclient
                httpclient.DefaultRequestHeaders.Add("AccessToken", authentication_token);


                // send request to authentication_check to login
                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(Config.VidmeUrlClass.AuthenticationCheckURL),
                    Method = HttpMethod.Post
                };

                await http_client_semaphore.WaitAsync();
                HttpResponseMessage msg =  await httpclient.SendAsync(request);
                http_client_semaphore.Release();

                string msg_string = await msg.Content.ReadAsStringAsync();

                var response_data = JsonConvert.DeserializeObject<Models.AuthenticationCheck.Rootobject>(msg_string);
                

                if(response_data.status)
                {
                    loggedIn = true;

                    var avatar_url = response_data.user.avatar_url;
                    avatar_url = avatar_url.Substring(0, avatar_url.LastIndexOf('?'));

                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        SignInIconImageContainer.Visibility = Visibility.Visible;
                        SignInIconImage.ImageSource = new BitmapImage(new Uri(avatar_url));

                        SignInIconIcon.Visibility = Visibility.Collapsed;

                        SignInText.Text = response_data.user.displayname;
                    });

                } else
                {
                    // Means auth token is invalid. remove value
                    vault.Remove(credentials);

                    http_client_semaphore.Wait();
                    //reset httpclient
                    configureHttpClient();
                    http_client_semaphore.Release();
                }


            }

        }

        void tryLogin() {
            if (http_client_task != null)
                http_client_task = http_client_task.ContinueWith((task) => tryLoginAsync());
            else
                http_client_task = tryLoginAsync();
        }

        void tryLogout()
        {
            if(http_client_task != null)
            {
                http_client_task.ContinueWith(async (task) =>
                {
                    configureHttpClient();
                    await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {

                        SignInIconImageContainer.Visibility = Visibility.Collapsed;

                        SignInIconIcon.Visibility = Visibility.Visible;

                        SignInText.Text = "Sign in";

                        loggedIn = false;
                    });

                    var credentials = getCredentialsFromLocker();

                    vault.Remove(credentials);

                });
            } else
            {
                configureHttpClient();
                SignInIconImageContainer.Visibility = Visibility.Collapsed;

                SignInIconIcon.Visibility = Visibility.Visible;

                SignInText.Text = "Sign in";

                loggedIn = false;

                var credentials = getCredentialsFromLocker();

                vault.Remove(credentials);
            }
        }

        async Task generateVideoFrameAsync() {

            /*HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new Uri(Config.VidmeUrlClass.FeaturedVideoURL),
                Method = HttpMethod.Post
            };

            await http_client_semaphore.WaitAsync();
            HttpResponseMessage msg = await httpclient.SendAsync(request);
            http_client_semaphore.Release();
            string msg_string = await msg.Content.ReadAsStringAsync();

            var response_data = JsonConvert.DeserializeObject<Models.Videos.Rootobject>(msg_string);

            List<Models.Videos.Video> videos = response_data.videos.ToList();*/

            IncrementalLoadingVideoList videos = new IncrementalLoadingVideoList(Config.VidmeUrlClass.FeaturedVideoURL, http_client_semaphore, httpclient);

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                MainFrame.Navigate(typeof(HomeFrame), videos);
                
            });
        }

        void generateVideoFrame()
        {
            if (http_client_task != null)
                http_client_task = http_client_task.ContinueWith((task) => generateVideoFrameAsync());
            else
                http_client_task = generateVideoFrameAsync();
        }

        public MainPage()
        {
            configureHttpClient();
            vault = new PasswordVault();
            loggedIn = false;

            tryLogin();

            this.InitializeComponent();


            generateVideoFrame();

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

        private async void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.IsEnabled = false;
            if (loggedIn)
            {
                dialog = new ContentDialog()
                {
                    Title = "Log out request",
                    Content = "Would you like to log out?",
                    PrimaryButtonText = "Log out",
                    SecondaryButtonText = "No",
                    RequestedTheme = ElementTheme.Dark
                };
                
                ContentDialogResult result = await dialog.ShowAsync();

                if(result == ContentDialogResult.Primary)
                {
                    tryLogout();
                }


            }
            else 
                try
                {
                    var webAuthenticationResult =
                        await Windows.Security.Authentication.Web.WebAuthenticationBroker.AuthenticateAsync(Windows.Security.Authentication.Web.WebAuthenticationOptions.None,
                        new Uri(Config.VidmeUrlClass.AuthenticationURL),
                        new Uri(Config.VidmeUrlClass.AuthenticationRedirectURL));
                switch (webAuthenticationResult.ResponseStatus)
                {
                    case WebAuthenticationStatus.Success:

                        string auth_code = webAuthenticationResult.ResponseData.ToString();
                            if (string.IsNullOrEmpty(auth_code) || auth_code == "denied") { }
                            else
                            {

                                auth_code = auth_code.Substring(auth_code.LastIndexOf('=') + 1);

                                var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", VidmeAuthentificationClass.Application_Key, VidmeAuthentificationClass.Application_Secret)));

                                HttpRequestMessage request = new HttpRequestMessage()
                                {
                                    RequestUri = new Uri(Config.VidmeUrlClass.AuthenticationTokenURL),
                                    Method = HttpMethod.Post
                                };

                                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);


                                var body = new List<KeyValuePair<string, string>>();
                                body.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                                body.Add(new KeyValuePair<string, string>("code", auth_code));

                                var content = new FormUrlEncodedContent(body);


                                request.Content = content;

                                await http_client_semaphore.WaitAsync();
                                HttpResponseMessage msg = await httpclient.SendAsync(request);
                                http_client_semaphore.Release();

                                var responsecontent = await msg.Content.ReadAsStringAsync();

                                var result = JsonConvert.DeserializeObject<Models.AuthenticationResponse.Rootobject>(responsecontent);


                                vault.Add(new PasswordCredential("vidmeforwindows", "vidme", result.auth.token));

                                var results = vault.Retrieve("vidmeforwindows", "vidme");

                                tryLogin();
                            }

                        break;
                    case WebAuthenticationStatus.ErrorHttp:
                            dialog = new ContentDialog()
                            {
                                Title = "Log In Error",
                                Content = "Could not log in.\nReason: " + webAuthenticationResult.ResponseErrorDetail,
                                PrimaryButtonText = "Ok",
                                RequestedTheme = ElementTheme.Dark
                            };
                            await dialog.ShowAsync();
                            
                            break;
                }



                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine("Error Occurred!");
                }

            button.IsEnabled = true;


        }





        private void Button_Click(object sender, RoutedEventArgs e) { }
        private void HomeButton_Click(object sender, RoutedEventArgs e) { }
        private void Button_Click_1(object sender, RoutedEventArgs e) { }
        private void DownloadButton_Click(object sender, RoutedEventArgs e) { }
        private void FeedButton_Click(object sender, RoutedEventArgs e) { }
        private void WatchLaterButton_Click(object sender, RoutedEventArgs e) { }
        private void TeamPicksButton_Click(object sender, RoutedEventArgs e) { }
        private void HotTodayButton_Click(object sender, RoutedEventArgs e) { }
        private void FreshUploadsButton_Click(object sender, RoutedEventArgs e) { }
        private void CreatorsButton_Click(object sender, RoutedEventArgs e) { }
        private void FollowingButton_Click(object sender, RoutedEventArgs e) { }
        private void FeaturedButton_Click(object sender, RoutedEventArgs e) { }
        private void SettingsButton_Click(object sender, RoutedEventArgs e) { }
        private void SearchButtonClick(object sender, RoutedEventArgs e) { }
        private void RefreshButton_Click(object sender, RoutedEventArgs e) { }

    }
}
