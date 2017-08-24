using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VidmeForWindows.Models.Album;

namespace VidmeForWindows.Utility
{
    class IncrementalLoadingAlbumList : IncrementalLoadingList<Models.Album.Album, Models.Album.Rootobject>
    {
        public IncrementalLoadingAlbumList(string url, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, http_client_semaphore, httpClient)
        {
        }

        public IncrementalLoadingAlbumList(string url, int limit, SemaphoreSlim http_client_semaphore, HttpClient httpClient) : base(url, limit, http_client_semaphore, httpClient)
        {
        }

        public override List<Album> extactData(Rootobject result)
        {
            return result.albums.Where((Album a) => !a._private ).ToList();
        }
    }
}
