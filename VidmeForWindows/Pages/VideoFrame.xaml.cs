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
using Windows.Media.Core;
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
    /// 

     public class VideoFrameParams
    {
        public HttpClient httpclient;
        public SemaphoreSlim http_client_semaphore;
        public List<Models.Videos.Video> videos;
    }

    public sealed partial class VideoFrame : Page, RefreshableFrameInterface
    {
        public HttpClient httpclient;
        public SemaphoreSlim http_client_semaphore;
        public List<Models.Videos.Video> videos;
        Models.Videos.Video current {
            get
            {
                return videos.ElementAt(position);
            }
        }
        event Action onLoaded;
        int position = 0;

        public VideoFrame()
        {
            this.InitializeComponent();
        }

        public object getPageParameter()
        {
            return new VideoFrameParams() {
                httpclient = httpclient,
                http_client_semaphore = http_client_semaphore,
                videos = videos
            };
        }

        public string getTitleText()
        {
            if (current.title != null)
                return current.title;
            else
                return "Video";
        }

        public bool isRefreshable()
        {
            return true;
        }

        public void loadedAction(Action loaded)
        {
            onLoaded += loaded;
        }

        public bool multipleSupported()
        {
            return false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var param = e.Parameter as VideoFrameParams;
            httpclient = param.httpclient;
            http_client_semaphore = param.http_client_semaphore;
            videos = param.videos;

            MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(current.complete_url));
            


            base.OnNavigatedTo(e);
            if (onLoaded != null)
                onLoaded();
        }


        private void setVideo(Models.Videos.Video video)
        {

        }
    }
}
