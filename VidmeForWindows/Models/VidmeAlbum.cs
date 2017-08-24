using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;

namespace VidmeForWindows.Models.Album
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Page page { get; set; }
        public Album[] albums { get; set; }
    }

    public class Page
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }

    public class Album
    {
        public string album_id { get; set; }
        public string user_id { get; set; }
        public string user_image_url { get; set; }
        public string url { get; set; }





        public string full_url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime date_created { get; set; }
        public int video_count { get; set; }
        public int view_count { get; set; }
        public bool _private { get; set; }
        public bool is_default { get; set; }



    }

   

}
