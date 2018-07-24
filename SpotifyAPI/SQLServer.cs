using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SpotifyApi 
{
    class SQLServer
    {
        public static SqlConnection connection { get; set; }

        public void openConnection()
        {
            string connectionString = Environment.GetEnvironmentVariable("SpotifyDBConnectionString");
            connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void closeConnection()
        {
            try
            {
                connection.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void addPlaylistToTable(string table_name, string playlistId)
        {
            table_name = "dbo." + table_name;
            SpotifyGetDataAPI p = new SpotifyGetDataAPI();
            spotifyPlaylist playlist = p.GetPlaylist(playlistId);
            foreach (songInfo song in playlist.items)
            {
                spotifyTrack t = song.track;
                string title = t.name.Replace("'", "''");
                List<string> artists = new List<string>();
                foreach (artistInfo a in t.artists)
                {
                    artists.Add(a.name);
                }
                string artistString = String.Join(", ", artists).Replace("'", "''");
                string album = t.album.name.Replace("'", "''");
                int @explicit = (t.@explicit == true) ? 1 : 0;
                TimeSpan duration = TimeSpan.FromMilliseconds(t.duration_ms);
                int popularity = t.popularity;
                string c = String.Format("INSERT INTO {0} (Title, Artist, Album, Explicit, Duration, Popularity) VALUES ('{1}', '{2}', '{3}', {4}, '{5}', {6});", table_name, title, artistString, album, @explicit, duration, popularity);
                SqlCommand command = new SqlCommand(c, connection);
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public List<object> getHighestOrLowest(string table_name, string column_name, bool descending)
        {
            List<object> trackData = new List<object>();
            string asc_or_desc = (descending) ? "DESC" : "ASC";
            string cmd = String.Format("EXEC dbo.sortTable @table_name = '{0}', @column_name = '{1}', @asc_or_desc = '{2}'", table_name, column_name, asc_or_desc);
            SqlCommand command = new SqlCommand(cmd, connection);
            SqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            if (reader != null && reader.HasRows && reader.Read())
            {
                for (int col = 0; col < reader.FieldCount; col++)
                {
                    trackData.Add(reader[col]);
                }
            }
            return trackData;
        }
    }
}
