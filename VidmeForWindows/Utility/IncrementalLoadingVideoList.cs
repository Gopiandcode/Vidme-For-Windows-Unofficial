using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace VidmeForWindows.Utility
{
    class IncrementalLoadingVideoList : ObservableCollection<Models.Videos.Video>, ISupportIncrementalLoading
    {
        private int offset = 0;
        private int limit = 10;

        public bool HasMoreItems
        {
            get { return hasmoreitems; }
        }
        private bool hasmoreitems = true;
        public string url;
        private SemaphoreSlim http_client_semaphore;
        private HttpClient httpClient;

        public IncrementalLoadingVideoList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient)
        {
            this.url = url;
            this.http_client_semaphore = http_client_semaphore;
            this.httpClient = httpClient;
        }

        public IncrementalLoadingVideoList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient)
        {
            this.limit = Math.Min(100,limit);
            this.url = url;
            this.http_client_semaphore = http_client_semaphore;
            this.httpClient = httpClient;
        }

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {

            ProgressBar progressRing = ((Window.Current.Content as Frame).Content as MainPage).contentLoadingRing;
            CoreDispatcher coreDispatcher = Window.Current.Dispatcher;

            return Task.Run<LoadMoreItemsResult>(async () =>
            {
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    progressRing.IsIndeterminate = true;
                });

                
                // Retrieve items
                string retrieve_url = url + "?limit=" + limit.ToString() + "&offset=" + offset.ToString();


                HttpRequestMessage request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(retrieve_url),
                    Method = HttpMethod.Post
                };

                await http_client_semaphore.WaitAsync();
                HttpResponseMessage msg = await httpClient.SendAsync(request);
                http_client_semaphore.Release();


                string msg_string = await msg.Content.ReadAsStringAsync();
                List<Models.Videos.Video> videos;

                try
                {
                    var response_data = JsonConvert.DeserializeObject<Models.Videos.Rootobject>(msg_string);

                   videos = response_data.videos.ToList();
                    offset += videos.Count;
                }
                catch (Exception e)
                {

                    videos = new List<Models.Videos.Video>();
                }

                if(videos.Count == 0)
                {
                    hasmoreitems = false;
                }
                
                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    foreach (Models.Videos.Video video in videos)
                    {
                        this.Add(video);
                    }

                    progressRing.IsIndeterminate = false;
                });

                return new LoadMoreItemsResult() { Count = count };

            }).AsAsyncOperation();

        }

        
    }
}
