using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.Serialization.Json;

namespace SpotifyApi 
{
    class SpotifyGetDataAPI
    {
        // create a public member variabke for each method to use
        public static string authCode { get; set; }

        public void getAccessToken()
        {
            string clientId = Environment.GetEnvironmentVariable("SpotifyClientId");
            string clientSecret = Environment.GetEnvironmentVariable("SpotifyClientSecret");

            var encodeIdSecret = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId + ":" + clientSecret));

            // connect to the spotify endpoint
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create("https://accounts.spotify.com/api/token");
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Accept = "application/json";
            webRequest.Headers.Add("Authorization: Basic " + encodeIdSecret);

            var request = ("grant_type=client_credentials");
            byte[] requestBytes = Encoding.ASCII.GetBytes(request);
            webRequest.ContentLength = requestBytes.Length;

            Stream stream = webRequest.GetRequestStream();
            stream.Write(requestBytes, 0, requestBytes.Length);
            stream.Close();

            // get the response
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            spotifyToken token;
            authCode = "";
            using (Stream responseStream = response.GetResponseStream())
            {
                // convert the JSON response to a string using the spotifyToken class -- @Program.cs Line: 35
                var serializer = new DataContractJsonSerializer(typeof(spotifyToken));
                token = (spotifyToken)serializer.ReadObject(responseStream);
                authCode = token.access_token;
            }
        }

        // method to request data from the specified endpoint
        public String makeRequest(string endPoint)
        {
            getAccessToken();

            String r = string.Empty;

            // connect to the endpoint
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(endPoint);
            request.PreAuthenticate = true;
            request.Headers.Add("Authorization", "Bearer " + authCode);
            request.Accept = "application/json";
            request.Method = "GET";
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine("ERROR: {0}", response.StatusCode);
                    }
                    else
                    {
                        using (Stream responseStream = response.GetResponseStream())
                        {
                            // read the response
                            StreamReader reader = new StreamReader(responseStream);
                            r = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error Occured\n:\\");
                Console.WriteLine(e);
            }

            return r;
        }

        // get a track using the track ID
        public spotifyTrack GetTrack(string id)
        {
            // make the connection to the track endpoint
            string endPoint = "https://api.spotify.com/v1/tracks/" + id;
            string response = makeRequest(endPoint);
            byte[] byteArray = Encoding.ASCII.GetBytes(response);
            MemoryStream stream = new MemoryStream(byteArray);

            // return a spotifyTrack class -- @Program.cs Line: 85
            var serializer = new DataContractJsonSerializer(typeof(spotifyTrack));
            spotifyTrack track = (spotifyTrack)serializer.ReadObject(stream);

            return track;
        }


        // get a playlist using the playlist ID and limit the tracks returned and the starting offset
        public spotifyPlaylist GetPlaylist(string id, int offset = 0, int limit = 0)
        {
            // make the connection to the endpoint
            string endPoint = "https://api.spotify.com/v1/users/ram_marwaha/playlists/" + id + "/tracks?offset=" + offset + ((limit == 0) ? "" : "&limit=" + limit);
            string response = makeRequest(endPoint);
            byte[] byteArray = Encoding.ASCII.GetBytes(response);
            MemoryStream stream = new MemoryStream(byteArray);

            // return a spotifyPlaylist class -- @Program.cs Line: 48
            var serializer = new DataContractJsonSerializer(typeof(spotifyPlaylist));
            spotifyPlaylist playlist = (spotifyPlaylist)serializer.ReadObject(stream);

            return playlist;
        }
    }
}
