using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidmeForWindows.Models.User;

namespace VidmeForWindows.Utility
{
    class IncrementalLoadingUserList : IncrementalLoadingList<Models.User.User, Models.User.Rootobject>
    {
        public IncrementalLoadingUserList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, http_client_semaphore, httpClient)
        {
        }

        public IncrementalLoadingUserList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, limit, http_client_semaphore, httpClient)
        {
        }

        public override string get_retrieve_url(string url, double limit, double offset)
        {
            return url + "limit=" + limit.ToString() + "&offset=" + offset.ToString();
        }

        public override List<User> extactData(Rootobject result)
        {
            return result.users.ToList();
        }
    }
}
