using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Challenge1.Models
{
    class RestHandler
    {
        public enum Actions { CreateMaze, GetMazeState, NextMove, GetMaze }
        private Dictionary<Actions, RequestInfo> _calls;

        public RestHandler()
        {
            _calls = new Dictionary<Actions, RequestInfo>()
            {
                { Actions.CreateMaze, new RequestInfo("POST", string.Empty ) },
                { Actions.GetMazeState, new RequestInfo("GET", "/{0}") },
                { Actions.NextMove, new RequestInfo("POST", "/{0}") },
                { Actions.GetMaze, new RequestInfo("GET", "/{0}/print") }
            };
        }

        public string Request(Actions action, JObject messageData = null)
        {
            string data = messageData?.ToString();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(_calls[action].Call);
            request.Method = _calls[action].Type;

            if (_calls[action].Type == "POST")
            {
                request.ContentType = "application/json";
                request.ContentLength = data.Length;
                StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
                requestWriter.Write(data);
                requestWriter.Close();
            }

            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();
            StreamReader responseReader = new StreamReader(webStream);
            string response = responseReader.ReadToEnd();
            responseReader.Close();

            if(action == Actions.CreateMaze && webResponse.StatusCode == HttpStatusCode.OK)
            {
                string mazeId = JObject.Parse(response).Value<string>("maze_id");

                _calls.Where(kvp => kvp.Key != Actions.CreateMaze).ToList().ForEach(kvp => kvp.Value.MazeId = mazeId);
            }

            return response;
        }

        private class RequestInfo
        {
            private readonly string URL = $"https://ponychallenge.trustpilot.com/pony-challenge/maze";

            public RequestInfo(string type, string options)
            {
                Type = type;
                Options = options;
            }

            public string MazeId { private get; set; }
            public string Type { get; private set; }
            public string Options { private get; set; }

            public string Call => string.IsNullOrEmpty(Options) ? URL : URL + string.Format(Options, MazeId);
        }
    }
}
