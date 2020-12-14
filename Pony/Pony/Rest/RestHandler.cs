// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.RestHandler.cs
// Created on: 20201211
// -----------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Pony.Rest
{
    public static class RestHandler
    {
        public enum RequestType
        {
            POST,
            GET
        }

        public static string Request(RequestUrl action, JObject messageData = null)
        {
            if (action == null) throw new Exception("Request action cannot be null");

            if (action.RequestType == RequestType.POST)
            {
                if (messageData == null) throw new Exception("Cannot send empty POST request");

                return Request(PostRequest(action.Call, messageData.ToString()));
            }

            return Request(GetRequest(action.Call));
        }

        private static HttpWebRequest GetRequest(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = nameof(RequestType.GET);

            return request;
        }

        private static HttpWebRequest PostRequest(Uri url, string messageData)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = nameof(RequestType.POST);

            request.ContentType   = "application/json";
            request.ContentLength = messageData.Length;
            var requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.ASCII);
            requestWriter.Write(messageData);
            requestWriter.Close();

            return request;
        }

        private static string Request(HttpWebRequest request)
        {
            string response;

            try
            {
                var webResponse    = (HttpWebResponse)request.GetResponse();
                var webStream      = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (WebException e)
            {
                if (e.InnerException?.Message == "No such host is known.")
                {
                    throw new WebException("Cannot connect to Trustpilot.");
                }

                if (!(e.Response is { } exResponse)) return null;

                var    resp              = new StreamReader(exResponse.GetResponseStream() ?? throw new InvalidOperationException());
                string messageFromServer = resp.ReadToEnd();
                resp.Close();

                throw new WebException($"{messageFromServer}");
            }

            return response;
        }
    }
}