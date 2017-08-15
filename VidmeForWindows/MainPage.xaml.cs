using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using VidmeForWindows.Dialogs;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
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
        public MainPage()
        {
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
            var dialog = new LoginDialog();

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
            }

            

        }
    }
}
