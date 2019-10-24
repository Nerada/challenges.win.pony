//-----------------------------------------------
//      Autor: Ramon Bollen
//       File: Challenge1.Rest.RestRequestor.cs
// Created on: 2019107
//-----------------------------------------------

using Challenge1.Support;

using Newtonsoft.Json.Linq;

using System;
using System.Net;
using System.Text;

namespace Challenge1.Rest
{
    /// <summary>
    /// Class for calling and analyzing Rest
    /// </summary>
    public class RestRequestor
    {
        private string _mazeId;

        public string CreateMaze(JObject payload)
        {
            if (payload == null)
            {
                throw new Exception("Cannot call function without payload");
            }

            try
            {
                string response = RestHandler.Request(new RequestURL(RequestURL.RestAction.CreateMaze), payload);

                _mazeId = JObject.Parse(response).Value<string>("maze_id");
                var createReturn = JObject.Parse(response);
                return createReturn.ToString();
            }
            catch (WebException e)
            {
                if (e.Message == "Only ponies can play")
                {
                    throw new InvalidPlayerNameException(e.Message);
                }
                throw new WebException(e.Message);
            }
        }

        public string RetrieveMaze()
        {
            if (string.IsNullOrEmpty(_mazeId))
            {
                throw new Exception("Create a maze first!");
            }

            return RestHandler.Request(new RequestURL(RequestURL.RestAction.GetMaze, _mazeId));
        }

        public string Move(string direction)
        {
            if (string.IsNullOrEmpty(_mazeId))
            {
                throw new Exception("Create a maze first!");
            }

            var directionPayload = new JObject()
            {
                { "direction", direction }
            };

            string response = RestHandler.Request(new RequestURL(RequestURL.RestAction.NextMove, _mazeId), directionPayload);

            return ParseMoveResult(response);
        }

        private static string ParseMoveResult(string response)
        {
            try
            {
                string state = JObject.Parse(response).SelectToken("state").ToString();

                if (state == "won" || state == "over")
                {
                    string hiddenURL = JObject.Parse(response).SelectToken("hidden-url").ToString();

                    string fileName = hiddenURL.Substring(0, hiddenURL.LastIndexOf('.')).Replace(@"/", "", StringComparison.InvariantCulture); ;
                    byte[] data = Convert.FromBase64String(fileName);
                    string decodedString = Encoding.UTF8.GetString(data);

                    string image = "/" + decodedString + ".jpg";

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