﻿// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.RestRequestor.cs
// Created on: 20221119
// -----------------------------------------------

using System;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using Pony.Support;

namespace Pony.Rest;

/// <summary>
///     Class for calling and analyzing Rest
/// </summary>
public class RestRequestor
{
    private string _mazeId;

    public string CreateMaze(JObject payload)
    {
        if (payload == null) throw new Exception("Cannot call function without payload");

        try
        {
            string response = RestHandler.Request(new RequestUrl(RequestUrl.RestAction.CreateMaze), payload);

            _mazeId = JObject.Parse(response).Value<string>("maze_id");
            JObject createReturn = JObject.Parse(response);
            return createReturn.ToString();
        }
        catch (WebException e)
        {
            if (e.Message == "Only ponies can play") throw new InvalidPlayerNameException(e.Message);

            throw new WebException(e.Message);
        }
    }

    public string RetrieveMaze()
    {
        if (string.IsNullOrEmpty(_mazeId)) throw new Exception("Create a maze first!");

        return RestHandler.Request(new RequestUrl(RequestUrl.RestAction.GetMaze, _mazeId));
    }

    public string Move(string direction)
    {
        if (string.IsNullOrEmpty(_mazeId)) throw new Exception("Create a maze first!");

        JObject directionPayload = new() {{"direction", direction}};

        string response = RestHandler.Request(new RequestUrl(RequestUrl.RestAction.NextMove, _mazeId), directionPayload);

        return ParseMoveResult(response);
    }

    private static string ParseMoveResult(string response)
    {
        try
        {
            string state = JObject.Parse(response).SelectToken("state")?.ToString();

            if (state is "won" or "over")
            {
                string hiddenUrl = JObject.Parse(response).SelectToken("hidden-url")?.ToString();

                if (hiddenUrl is not { } url) return null;

                string fileName = url.Substring(0, url.LastIndexOf('.'))
                                     .Replace(@"/", "", StringComparison.InvariantCulture);

                byte[] data          = Convert.FromBase64String(fileName);
                string decodedString = Encoding.UTF8.GetString(data);

                string image = "/" + decodedString + ".jpg";

                return image;
            }

            return JObject.Parse(response).SelectToken("state-result")?.ToString();
        }
        catch { throw new Exception(); }
    }
}