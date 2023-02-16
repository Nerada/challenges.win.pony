// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.RestHandler.cs
// Created on: 20221119
// -----------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

#pragma warning disable SYSLIB0014

namespace Pony.Rest;

public static class RestHandler
{
    public enum RequestType
    {
        POST,
        GET
    }

    public static string Request(RequestUrl action, JObject messageData = null)
    {
        if (action == null) throw new InvalidOperationException("Request action cannot be null");

        if (action.RequestType == RequestType.POST)
        {
            if (messageData == null) throw new InvalidOperationException("Cannot send empty POST request");

            return Request(PostRequest(action.Call, messageData.ToString()));
        }

        return Request(GetRequest(action.Call));
    }

    private static HttpWebRequest GetRequest(Uri url)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = nameof(RequestType.GET);

        return request;
    }

    private static HttpWebRequest PostRequest(Uri url, string messageData)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = nameof(RequestType.POST);

        request.ContentType   = "application/json";
        request.ContentLength = messageData.Length;
        StreamWriter requestWriter = new(request.GetRequestStream(), Encoding.ASCII);
        requestWriter.Write(messageData);
        requestWriter.Close();

        return request;
    }

    private static string Request(WebRequest request)
    {
        string response = string.Empty;

        try
        {
            HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse();
            Stream          webStream   = webResponse.GetResponseStream();
            if (webStream != null)
            {
                StreamReader responseReader = new(webStream);
                response = responseReader.ReadToEnd();
                responseReader.Close();
            }
        }
        catch (WebException e)
        {
            if (e.InnerException?.Message == "No such host is known.") throw new WebException("Cannot connect to remote host.");

            if (e.Response is not { } exResponse) return null;

            StreamReader resp              = new(exResponse.GetResponseStream() ?? throw new InvalidOperationException());
            string       messageFromServer = resp.ReadToEnd();
            resp.Close();

            throw new WebException($"{messageFromServer}");
        }

        return response;
    }
}