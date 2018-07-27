using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SpotifyApi
{
    class Program
    {
        static void Main(string[] args)
        {
            SpotifyGetDataAPI spotify = new SpotifyGetDataAPI();

            spotifyPlaylist americannn = spotify.GetPlaylist("5ybgdI5zfh6NcMDQp5NHVk");
            spotifyTrack champions = spotify.GetTrack("3eZGNBvKkExXp8q5FAyM8y");
            
            Console.WriteLine("Americannn Playlist:");
            foreach (songInfo t in americannn.items)
            {
                Console.WriteLine("Title: {0}\tArtist: {1}\t Album: {2}", t.track.name, t.track.artists[0].name, t.track.album.name);
            }
            Console.WriteLine("\nChampions Track Details:");
            List<string> artists = new List<string>();
            foreach (artistInfo artist in champions.artists)
            {
                artists.Add(artist.name);
            }
            Console.WriteLine("Artists: {0}\nAlbum: {1}\nExplicit: {2}\nRating: {3}\nDuration: {4}", String.Join(", ", artists), champions.album.name, champions.@explicit, champions.popularity, TimeSpan.FromMilliseconds(champions.duration_ms));
            
            SQLServer server = new SQLServer();
            server.openConnection();
            server.addPlaylistToTable("Americannn", "5ybgdI5zfh6NcMDQp5NHVk");
            server.addPlaylistToTable("Bangerzzz", "0quqcsWCf5xt8z76kMFMzf");
            List<object> track = server.getHighestOrLowest("Americannn", "Popularity", true);
            Console.WriteLine("Most Popular Song in Americannn Playlist: {0} by {1}", track[1], track[2]);
            List<object> t = server.getHighestOrLowest("", "ReleaseDate", false);
            Console.WriteLine("Latest Song in All Playlists: {0} by {1}", t[1], t[2]);
            server.closeConnection();
        }
    }

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
