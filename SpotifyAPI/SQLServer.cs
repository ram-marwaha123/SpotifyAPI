using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace SpotifyApi 
{
    class SQLServer
    {
        // create a public member variable that is used throughout the class
        public static SqlConnection connection { get; set; }


        // method to connect to the databse server
        public void openConnection()
        {
            // connect to server with connection string
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

        // method to close the connection
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

        // method to create a stand alone playlist table
        public void createPlaylistTable(string table_name)
        {
            // execute SQL stored procedure
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

        // method to add playlist to table
        public void addPlaylistToTable(string playlist_name, string playlistId)
        {
            SpotifyGetDataAPI p = new SpotifyGetDataAPI();
            spotifyPlaylist playlist = p.GetPlaylist(playlistId);                       // get the playlist using the ID provided -- @Program.cs Line: 48
            foreach (songInfo song in playlist.items)
            {
                spotifyTrack t = song.track;                                            // get the track from the playlist using class -- @Program.cs Line: 85
                string title = t.name.Replace("'", "''");
                List<string> artists = new List<string>();
                foreach (artistInfo a in t.artists)
                {
                    artists.Add(a.name);                                                // add the artists to a list
                }
                string artistString = String.Join(", ", artists).Replace("'", "''");
                string album = t.album.name.Replace("'", "''");                         // get the album name from spotifyAlbum class
                int @explicit = (t.@explicit) ? 1 : 0;                                  // get the boolean value to check if the track is explicit
                TimeSpan duration = TimeSpan.FromMilliseconds(t.duration_ms);           // get the duration
                int popularity = t.popularity;                                          // get the popularity 
                string trackId = t.id.Replace("'", "''");                               // get the track ID
                string release_date = t.album.release_date;                             // get the album release date from the spotifyAlbum class

                // execute the SQL stored procedure
                string c = String.Format("EXEC dbo.insertPlaylistData @Playlist = '{0}', @Title = '{1}', @Artist = '{2}', @Album = '{3}', @Explicit = {4}, @Duration = '{5}', @Popularity = {6}, @Release_Date = '{7}', @ID = '{8}'", playlist_name, title, artistString, album, @explicit, duration, popularity, release_date, trackId);
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

        // method to return information about a certain track. E.g. the most popular track or the shortest track
        public List<object> getHighestOrLowest(string playlist_name, string column_name, bool descending)
        {
            List<object> trackData = new List<object>();
            string asc_or_desc = (descending) ? "DESC" : "ASC";         // convert the boolean to a string of an SQL command

            // execute the SQL stored procedure to sort the table
            string cmd = String.Format("EXEC dbo.sortTable @playlist_name = '{0}', @column_name = '{1}', @asc_or_desc = '{2}'", playlist_name, column_name, asc_or_desc);
            SqlCommand command = new SqlCommand(cmd, connection);

            // read the data
            SqlDataReader reader = null;
            try
            {
                reader = command.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            // only read the first row
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
