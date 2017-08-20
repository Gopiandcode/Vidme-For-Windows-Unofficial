using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Models.Channels
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Channel[] data { get; set; }
    }

    public class Channel
    {
        public string channel_id { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string date_created { get; set; }
        public bool is_default { get; set; }
        public bool hide_suggest { get; set; }
        public bool show_unmoderated { get; set; }
        public bool nsfw { get; set; }
        public int follower_count { get; set; }
        public int video_count { get; set; }
        public string full_url { get; set; }
        public string avatar_url { get; set; }
        public string avatar_ai { get; set; }
        public string cover_url { get; set; }
        public string cover_ai { get; set; }
    }

}
