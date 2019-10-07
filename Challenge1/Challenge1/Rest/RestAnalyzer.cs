using Challenge1.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text;

namespace Challenge1.Rest
{
    /// <summary>
    /// Class for calling and analyzing Rest
    /// </summary>
    internal class RestAnalyzer
    {
        private RestHandler _restHandler = new RestHandler();

        public string CreateMaze(JObject payload)
        {
            return JObject.Parse(_restHandler.Request(RestHandler.Actions.CreateMaze, payload)).ToString();
        }

        public string RetrieveMaze()
        {
            return _restHandler.Request(RestHandler.Actions.GetMaze);
        }

        public string Move(string direction)
        {
            JObject directionPayload = new JObject();
            directionPayload.Add("direction", direction);

            string response = _restHandler.Request(RestHandler.Actions.NextMove, directionPayload);

            try
            {
                string state = JObject.Parse(response).SelectToken("state").ToString();

                if (state == "won" || state == "over")
                {
                    string hiddenURL = JObject.Parse(response).SelectToken("hidden-url").ToString();

                    string fileName = hiddenURL.Substring(0, hiddenURL.LastIndexOf('.')).Replace(@"/", "", StringComparison.InvariantCulture); ;
                    byte[] data = Convert.FromBase64String(fileName);
                    string decodedString = Encoding.UTF8.GetString(data);

                    string image = "/"+decodedString+".jpg";

                    return image;
                }

                return JObject.Parse(response).SelectToken("state-result").ToString();
            }
            catch 
            {
                throw new Exception();
            }
        }
    }
}