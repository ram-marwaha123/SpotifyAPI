using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpotifyApi
{
    class Program
    {
        static void Main(string[] args)
        {
            // initialise server class
            SQLServer server = new SQLServer();

            // connect to databse server
            server.openConnection();

            // add playlists to table
            server.addPlaylistToTable("Americannn", "5ybgdI5zfh6NcMDQp5NHVk");
            server.addPlaylistToTable("Bangerzzz", "0quqcsWCf5xt8z76kMFMzf");

            // get data from table and display
            List<object> track = server.getHighestOrLowest("Americannn", "Popularity", true);
            Console.WriteLine("Most Popular Song in Americannn Playlist: {0} by {1}", track[1], track[2]);
            List<object> t = server.getHighestOrLowest("", "ReleaseDate", false);
            Console.WriteLine("Latest Song in All Playlists: {0} by {1}", t[1], t[2]);

            // close the server connection
            server.closeConnection();
        }
    }

    // ------ CLASSES USED FOR INTERPRETING JSON RESPONSE FROM API ------

    [DataContract]
    class spotifyToken
    {
        [DataMember]
        public string access_token { get; set; }
        [DataMember]
        public string token_type { get; set; }
        [DataMember]
        public int expires_in { get; set; }
        [DataMember]
        public string scope { get; set; }
    }

    [DataContract]
    class spotifyPlaylist
    {
        [DataMember]
        public string href { get; set; }
        [DataMember]
        public songInfo[] items { get; set; }
        [DataMember]
        public int limit { get; set; }
        [DataMember]
        public string next { get; set; }
        [DataMember]
        public int offset { get; set; }
        [DataMember]
        public string previous { get; set; }
        [DataMember]
        public int total { get; set; }
    }

    [DataContract]
    class songInfo
    {
        [DataMember]
        public string added_at { get; set; }
        [DataMember]
        public object added_by { get; set; }
        [DataMember]
        public bool is_local { get; set; }
        [DataMember]
        public string primary_color { get; set; }
        [DataMember]
        public spotifyTrack track { get; set; }
        [DataMember]
        public object video_thumbnail { get; set; }

    }

    [DataContract]
    class spotifyTrack
    {
        [DataMember]
        public spotifyAlbum album { get; set; }
        [DataMember]
        public artistInfo[] artists { get; set; }
        [DataMember]
        public object available_markets { get; set; }
        [DataMember]
        public int disc_number { get; set; }
        [DataMember]
        public long duration_ms { get; set; }
        [DataMember]
        public bool @explicit { get; set; }
        [DataMember]
        public object external_ids { get; set; }
        [DataMember]
        public object external_urls { get; set; }
        [DataMember]
        public string href { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public bool is_local { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public int popularity { get; set; }
        [DataMember]
        public string preview_uri { get; set; }
        [DataMember]
        public int track_number { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string uri { get; set; }
    }

    [DataContract]
    class artistInfo
    {
        [DataMember]
        public object external_urls { get; set; }
        [DataMember]
        public string href { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string uri { get; set; }
    }

    [DataContract]
    class spotifyAlbum
    {
        [DataMember]
        public string album_type { get; set; }
        [DataMember]
        public artistInfo[] artists { get; set; }
        [DataMember]
        public object available_markets { get; set; }
        [DataMember]
        public object external_urls { get; set; }
        [DataMember]
        public string href { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public object images { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public string release_date { get; set; }
        [DataMember]
        public string release_date_precision { get; set; }
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string uri { get; set; }
    }
}
