//-----------------------------------------------
//      Author: Ramon Bollen
//       File: Challenge1.Rest.RestHandler.cs
// Created on: 2019107
//-----------------------------------------------

using Newtonsoft.Json.Linq;

using System;
using System.IO;
using System.Net;

namespace Challenge1.Rest
{
    public static class RestHandler
    {
        public enum RequestType
        {
            POST,
            GET
        }

        public static string Request(RequestURL action, JObject messageData = null)
        {
            if (action == null)
            {
                throw new Exception("Request action cannot be null");
            }

            if (action.RequestType == RequestType.POST)
            {
                if (messageData == null)
                {
                    throw new Exception("Cannot send empty POST request");
                }

                return Request(PostRequest(action.Call, messageData.ToString()));
            }
            else
            {
                return Request(GetRequest(action.Call));
            }

            throw new Exception("Unhandled request type");
        }

        private static HttpWebRequest GetRequest(Uri url)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri: url);
            request.Method = RequestType.GET.ToString();

            return request;
        }

        private static HttpWebRequest PostRequest(Uri url, string messageData)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri: url);
            request.Method = RequestType.POST.ToString();

            request.ContentType = "application/json";
            request.ContentLength = messageData.Length;
            var requestWriter = new StreamWriter(request.GetRequestStream(), System.Text.Encoding.ASCII);
            requestWriter.Write(messageData);
            requestWriter.Close();

            return request;
        }

        private static string Request(HttpWebRequest request)
        {
            string response;

            try
            {
                var webResponse = (HttpWebResponse)request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();
                var responseReader = new StreamReader(webStream);
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
            catch (WebException e)
            {
                if (e.InnerException != null && e.InnerException.Message == "No such host is known.")
                {
                    throw new WebException("Cannot connect to Trustpilot.");
                }

                var resp = new StreamReader(e.Response.GetResponseStream());
                string messageFromServer = resp.ReadToEnd();
                resp.Close();

                throw new WebException($"{messageFromServer}");
            }

            return response;
        }
    }
}