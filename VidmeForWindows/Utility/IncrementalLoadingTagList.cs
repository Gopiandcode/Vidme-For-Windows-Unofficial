using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidmeForWindows.Models.Label;

namespace VidmeForWindows.Utility
{
    class IncrementalLoadingTagList : IncrementalLoadingList<Models.Label.Tag, Models.Label.Rootobject>
    {
        public IncrementalLoadingTagList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, http_client_semaphore, httpClient)
        {
        }

        public IncrementalLoadingTagList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, limit, http_client_semaphore, httpClient)
        {
        }

        override public string get_retrieve_url(string url, double limit, double offset)
        {
            return url + "limit=" + limit.ToString() + "&offset=" + offset.ToString();
        }

        public override List<Tag> extactData(Rootobject result)
        {
            return result.tags.ToList();
        }
    }
}
