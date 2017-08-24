using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Models.Comment
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Page page { get; set; }
        public Comment[] comments { get; set; }
    }

    public class Page
    {
        public int total { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
    }

    public class Comment
    {
        public string comment_id { get; set; }
        public string video_id { get; set; }
        public object album_id { get; set; }
        public string user_id { get; set; }
        public string parent_comment_id { get; set; }
        public string full_url { get; set; }
        public string body { get; set; }
        public string date_created { get; set; }
        public float? made_at_time { get; set; }
        public int score { get; set; }
        public int comment_count { get; set; }
        public User user { get; set; }
        public Viewervote viewerVote { get; set; }
        public Video video { get; set; }
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
        public bool vip { get; set; }
    }

    public class Viewervote
    {
        public string vote_id { get; set; }
        public string comment_id { get; set; }
        public string video_id { get; set; }
        public object album_id { get; set; }
        public string user_id { get; set; }
        public int value { get; set; }
        public string date_created { get; set; }
        public string date_modified { get; set; }
    }

    public class Video
    {
        public string video_id { get; set; }
        public string url { get; set; }
        public string full_url { get; set; }
        public string embed_url { get; set; }
        public string user_id { get; set; }
        public string complete { get; set; }
        public string complete_url { get; set; }
        public string state { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public float duration { get; set; }
        public int height { get; set; }
        public int width { get; set; }
        public string date_created { get; set; }
        public string date_stored { get; set; }
        public string date_completed { get; set; }
        public string date_published { get; set; }
        public string scheduled { get; set; }
        public bool subscribed_only { get; set; }
        public int comment_count { get; set; }
        public int view_count { get; set; }
        public int share_count { get; set; }
        public int watching_count { get; set; }
        public int version { get; set; }
        public bool nsfw { get; set; }
        public string thumbnail { get; set; }
        public string thumbnail_url { get; set; }
        public string thumbnail_gif { get; set; }
        public string thumbnail_gif_url { get; set; }
        public string thumbnail_ai { get; set; }
        public string storyboard { get; set; }
        public int score { get; set; }
        public int likes_count { get; set; }
        public string channel_id { get; set; }
        public string source { get; set; }
        public bool _private { get; set; }
        public int latitude { get; set; }
        public int longitude { get; set; }
        public object place_id { get; set; }
        public object place_name { get; set; }
        public object colors { get; set; }
        public object reddit_link { get; set; }
        public object youtube_override_source { get; set; }
        public User user { get; set; }
        public Channel channel { get; set; }
        public Format[] formats { get; set; }
        public string total_tipped { get; set; }
        public bool is_featured { get; set; }
        public string date_featured { get; set; }
        public Creator creator { get; set; }
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

    public class Creator
    {
        public string source { get; set; }
        public string id { get; set; }
        public string uri { get; set; }
        public string title { get; set; }
        public string avatar { get; set; }
    }

    public class Format
    {
        public string type { get; set; }
        public string uri { get; set; }
        public object width { get; set; }
        public object height { get; set; }
        public int version { get; set; }
    }

}
