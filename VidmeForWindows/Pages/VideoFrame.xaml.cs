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
using Windows.Media.Core;
using Windows.Media.Streaming.Adaptive;
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
        public List<Models.Videos.Video> videos = new List<Models.Videos.Video>();
        Task current_video_task;
        Models.Videos.Video current {
            get
            {
                if (videos.Count != 0)
                    return videos.ElementAt(position);
                else
                    return new Models.Videos.Video()
                    {
                    };
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
            setVideo(current);


            base.OnNavigatedTo(e);
            if (onLoaded != null)
                onLoaded();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if(MediaPlayer.MediaPlayer != null)
            MediaPlayer.MediaPlayer.Pause();
            base.OnNavigatedFrom(e);
        }

        private void setVideo(Models.Videos.Video video)
        {
            if (current_video_task != null)
            {
                current_video_task.ContinueWith((Task t) =>
                {
                    t.Wait();
                    return setVideoAsync(video);
                });
            }
            else
                current_video_task = setVideoAsync(video);
                 }

        private async Task setVideoAsync(Models.Videos.Video video)
        {
            AdaptiveMediaSourceCreationResult src = await AdaptiveMediaSource.CreateFromUriAsync(new Uri(video.complete_url));
            await Window.Current.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal ,() => {

                MoreFromListView.ItemsSource = new IncrementalLoadingVideoList(Config.VidmeUrlClass.UserVideoURL(video.user_id), http_client_semaphore, httpclient);

                if (src.Status == AdaptiveMediaSourceCreationStatus.Success) {
                    MediaPlayer.SetMediaPlayer(new Windows.Media.Playback.MediaPlayer());

                    MediaPlayer.MediaPlayer.Source = MediaSource.CreateFromAdaptiveMediaSource(src.MediaSource);

                } else
                {
                    MediaPlayer.SetMediaPlayer(new Windows.Media.Playback.MediaPlayer());
                    MediaPlayer.MediaPlayer.Source = MediaSource.CreateFromUri(new Uri(video.complete_url));
                }

            });
            

        }

        private void PlaylistListView_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}
