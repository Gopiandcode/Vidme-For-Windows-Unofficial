using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidmeForWindows.Models.Label
{

    public class Rootobject
    {
        public bool status { get; set; }
        public Page page { get; set; }
        public Tag[] tags { get; set; }
    }

    public class Page
    {
        public int offset { get; set; }
        public int limit { get; set; }
    }

    public class Tag
    {
        public string tag_id { get; set; }
        public string text { get; set; }
        public string label { get; set; }
        public string date_created { get; set; }
        public int use_count { get; set; }
    }

}
