using System;
using System.Collections.Generic;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace VidmeForWindows.Pages
{

    public class TagFrameParams
    {
        public HttpClient httpClient;
        public SemaphoreSlim http_client_semaphore;
    }

    public enum State
    {
        SEARCH,
        HOT
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TagFrame : Page
    {
        State current_state;
        HttpClient client;
        SemaphoreSlim http_client_semaphore;
        public TagFrame()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TagFrameParams para = e.Parameter as TagFrameParams;

            client = para.httpClient;
            http_client_semaphore = para.http_client_semaphore;

            setState(State.HOT, null, true);
            // initialize list

            base.OnNavigatedTo(e);
        }



        void setState(State state, string url)
        {
            setState(state, url, false);
        }

        void setState(State state, string url, bool ignorestate)
        {
            if ((state == State.HOT && state == current_state) && !ignorestate) return;
            else
            {
                switch(state)
                {
                    case State.HOT:
                        TagListView.ItemsSource = new Utility.IncrementalLoadingTagList(VidmeForWindows.Config.VidmeUrlClass.TagListURL + "?", http_client_semaphore, client);
                        break;
                    case State.SEARCH:
                        TagListView.ItemsSource = new Utility.IncrementalLoadingTagList(VidmeForWindows.Config.VidmeUrlClass.TagSearchURL(url) + "&", http_client_semaphore, client);
                        break;
                }
            }
            current_state = state;
        }

        void updateSearch()
        {
            string search = SearchTextBox.Text;
            if (string.IsNullOrWhiteSpace(search))
            {
                // update itemsource
                setState(State.HOT, null);
            }
            else if (!string.IsNullOrEmpty(search))
            {
                string escaped_string = Uri.EscapeDataString(search);
                // update url
                setState(State.SEARCH, escaped_string);

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

        private void TagClick(object sender, ItemClickEventArgs e)
        {
            Models.Label.Tag tag = e.ClickedItem as Models.Label.Tag;
            
            ((Window.Current.Content as Frame).Content as MainPage).displayTagVideos(Config.VidmeUrlClass.TagVideoURL(tag.tag_id), tag.label + " Tags");
        }
    }
}
