using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Models.AuthenticationCheck
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Auth auth { get; set; }
        public User user { get; set; }
        public Application application { get; set; }
    }

    public class Auth
    {
        public string token { get; set; }
        public string expires { get; set; }
        public string user_id { get; set; }
        public string application_id { get; set; }
        public string scopes { get; set; }
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
    }

    public class Application
    {
        public string application_id { get; set; }
        public string client_id { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string organization { get; set; }
        public string redirect_uri_prefix { get; set; }
    }


}
