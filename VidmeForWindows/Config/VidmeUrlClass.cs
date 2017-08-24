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

        public static string ChannelURL = "https://api.vid.me/channels";

        public static string FollowingUserURL(string id) => "https://api.vid.me/user/" + id + "/following";

        public static string FollowersUserURL(string id) => "https://api.vid.me/user/" + id + "/followers";

        public static string FeaturedUserURL = "https://api.vid.me/users/featured";

        public static string SearchUserURL(string text) => "https://api.vid.me/users/suggest?text=" + text;

        public static string TagListURL = "https://api.vid.me/tags/list";

        public static string TagSearchURL(string text) => "https://api.vid.me/tags/suggest?text=" + text;

        public static string TagVideoURL(string id) => "https://api.vid.me/tag/"+ id + "/hot";

        public static string UserVideoURL(string id) => "https://api.vid.me/user/" + id + "/videos";

        public static string CheckSubscribedURL(string id) => "https://api.vid.me/user/" + id + "/subscribed";

        public static string LikedVideosUserURL(string id) => "https://api.vid.me/videos/likes?user=" + id;

        public static string UserAlbumsURL(string id) => "https://api.vid.me/user/" + id + "/albums";

        public static string AlbumVideosURL(string id) => "https://api.vid.me/album/" + id + "/videos";

    }
}
