using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Config
{
    class VidmeUrlClass
    {
        public static string AuthenticationURL = "https://vid.me/oauth/authorize?client_id=" + VidmeAuthentificationClass.Application_ClientID + "&redirect_uri=http%3A%2F%2Fgopiandcode.com%2FVidmeForWindows&scopes=votes%20comments%20channels%20basic&response_type=code";
        public static string AuthenticationRedirectURL = "http://gopiandcode.com/VidmeForWindows";

        public static string AuthenticationTokenURL = "https://api.vid.me/oauth/token";

        public static string AuthenticationCheckURL = "https://api.vid.me/auth/check";

        public static string FeaturedVideoURL = "https://api.vid.me/videos/featured";

        public static string FreshVideoURL = "https://api.vid.me/videos/new";

        public static string FeedVideoURL = "https://api.vid.me/videos/following";

        public static string HotVideoURL = "https://api.vid.me/videos/hot/main";

        public static string TeamPickVideoURL = "https://api.vid.me/videos/hot/vip";

    }
}
