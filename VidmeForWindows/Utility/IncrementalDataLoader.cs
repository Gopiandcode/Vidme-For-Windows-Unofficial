using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public abstract class IncrementalLoadingList<T,P> : ObservableCollection<T>, ISupportIncrementalLoading
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

        public IncrementalLoadingList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient)
        {
            this.url = url;
            this.http_client_semaphore = http_client_semaphore;
            this.httpClient = httpClient;
        }

        public IncrementalLoadingList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient)
        {
            this.limit = Math.Min(100, limit);
            this.url = url;
            this.http_client_semaphore = http_client_semaphore;
            this.httpClient = httpClient;
        }

        public abstract List<T> extactData(P result);

        public virtual string get_retrieve_url(string url, double limit, double offset)
        {
            return url + "?limit=" + limit.ToString() + "&offset=" + offset.ToString();
        }

        public virtual HttpRequestMessage get_request_message(string url)
        {
            return new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Post
            };

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
                string retrieve_url = get_retrieve_url(url, limit, offset);


                HttpRequestMessage request = get_request_message(retrieve_url);

                await http_client_semaphore.WaitAsync();
                HttpResponseMessage msg = await httpClient.SendAsync(request);
                http_client_semaphore.Release();


                string msg_string = await msg.Content.ReadAsStringAsync();
                List<T> videos;

                try
                {
                    var response_data = JsonConvert.DeserializeObject<P>(msg_string);

                    videos = extactData(response_data);
                    offset += videos.Count;
                }
                catch (Exception e)
                {

                    videos = new List<T>();
                }

                if (videos.Count == 0)
                {
                    hasmoreitems = false;
                }

                await coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    foreach (T video in videos)
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
