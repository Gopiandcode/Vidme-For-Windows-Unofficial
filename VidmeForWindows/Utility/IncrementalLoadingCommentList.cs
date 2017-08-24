using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidmeForWindows.Models.Comment;

namespace VidmeForWindows.Utility
{
    class IncrementalLoadingCommentList : IncrementalLoadingList<Models.Comment.Comment, Models.Comment.Rootobject>
    {
        public IncrementalLoadingCommentList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, http_client_semaphore, httpClient)
        {
        }

        public IncrementalLoadingCommentList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, limit, http_client_semaphore, httpClient)
        {
        }

        public override HttpRequestMessage get_request_message(string url)
        {
            return new HttpRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

        }

        public override List<Comment> extactData(Rootobject result)
        {
            return result.comments.ToList();
        }
    }
}
