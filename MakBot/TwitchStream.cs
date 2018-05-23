using System;
using System.Collections.Generic;
using System.Text;

namespace MakBot
{
    // Created this using paste JSON to Class in VS with edits

    /// <summary>
    /// Represents the JSON response for a given twitch stream
    /// </summary>
    public class TwitchStream
    {
        public StreamData[] Data { get; set; }
        public Pagination Pagination { get; set; }
    }

    /// <summary>
    /// Represents the bulk of the data concerning the stream
    /// </summary>
    public class StreamData
    {
        public string[] Community_ids { get; set; }
        public string Game_id { get; set; }
        public string Id { get; set; }
        public string Language { get; set; }
        public string Started_at { get; set; }
        public string Thumbnail_url { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string User_id { get; set; }
        public int Viewer_count { get; set; }
    }

    /// <summary>
    /// I don't know what this is
    /// </summary>
    public class Pagination
    {
        public string Cursor { get; set; }
    }
}

