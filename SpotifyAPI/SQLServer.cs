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

        public void createPlaylistTable(string table_name)
        {
            string cmd = "EXEC dbo.newPlaylistTable @name = " + table_name;
            SqlCommand command = new SqlCommand(cmd, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void addPlaylistToTable(string table_name, string playlistId)
        {
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
                string trackId = t.id.Replace("'", "''");
                string release_date = t.album.release_date;
                string c = String.Format("EXEC dbo.insertPlaylistData @Playlist = '{0}', @Title = '{1}', @Artist = '{2}', @Album = '{3}', @Explicit = {4}, @Duration = '{5}', @Popularity = {6}, @Release_Date = '{7}', @ID = '{8}'", table_name, title, artistString, album, @explicit, duration, popularity, release_date, trackId);
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

        public List<object> getHighestOrLowest(string playlist_name, string column_name, bool descending)
        {
            List<object> trackData = new List<object>();
            string asc_or_desc = (descending) ? "DESC" : "ASC";
            string cmd = String.Format("EXEC dbo.sortTable @playlist_name = '{0}', @column_name = '{1}', @asc_or_desc = '{2}'", playlist_name, column_name, asc_or_desc);
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
            reader.Close();
            return trackData;
        }
    }
}
