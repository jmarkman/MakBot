using System;
using System.Collections.Generic;
using System.Text;

namespace MakBot
{
    public class ClipData
    {
        public List<Clip> Clips { get; set; }
    }

    public class Clip
    {
        public string Shorthand { get; set; }
        public string Filename { get; set; }
        public string Summary { get; set; }
    }

}
