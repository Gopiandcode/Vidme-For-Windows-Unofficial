using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Models.User
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Page page { get; set; }
        public User[] users { get; set; }
    }

    public class Page
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }

    public class User
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string full_url { get; set; }
        public string avatar { get; set; }
        public string avatar_url { get; set; }
        public string avatar_ai { get; set; }
        public string cover { get; set; }
        public string cover_url { get; set; }
        public string cover_ai { get; set; }
        public string displayname { get; set; }
        public int follower_count { get; set; }
        public string likes_count { get; set; }
        public int video_count { get; set; }
        public string video_views { get; set; }
        public int videos_scores { get; set; }
        public int comments_scores { get; set; }
        public string bio { get; set; }
        public object ga_id { get; set; }
        public bool is_following { get; set; }
        public bool is_followed_by { get; set; }
        public bool receive_notifications_following { get; set; }
        public bool receive_notifications_followed { get; set; }
        public bool is_subscribed { get; set; }
        public bool is_subscribed_by { get; set; }
        public bool vip { get; set; }
    }

}
