//-----------------------------------------------
//      Author: Ramon Bollen
//       File: Challenge1.Rest.RequestURL.cs
// Created on: 20191014
//-----------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;

using static Challenge1.Rest.RestHandler;

namespace Challenge1.Rest
{
    public class RequestURL
    {
        public enum RestAction { CreateMaze, NextMove, GetMazeState, GetMaze }

        private readonly Uri URLStart = new Uri($"https://ponychallenge.trustpilot.com/pony-challenge/");

        private readonly RestAction _chosenAction;

        private readonly string     _mazeId;

        private readonly Dictionary<RestAction, string> _calls = new Dictionary<RestAction, string>()
        {
            { RestAction.CreateMaze,   "maze" },
            { RestAction.GetMazeState, "maze/{0}" },
            { RestAction.NextMove,     "maze/{0}" },
            { RestAction.GetMaze,      "maze/{0}/print" }
        };

        private readonly Dictionary<RestAction, RequestType> _type = new Dictionary<RestAction, RequestType>()
        {
            { RestAction.CreateMaze,   RequestType.POST },
            { RestAction.GetMazeState, RequestType.GET },
            { RestAction.NextMove,     RequestType.POST },
            { RestAction.GetMaze,      RequestType.GET }
        };

        public RequestURL(RestAction action, string mazeId = "")
        {
            if (action != RestAction.CreateMaze && string.IsNullOrEmpty(mazeId))
            {
                throw new Exception("Maze Id is required for this type of action.");
            }

            _chosenAction = action;
            _mazeId = mazeId;
        }

        public Uri Call => new Uri(URLStart, string.Format(CultureInfo.InvariantCulture, _calls[_chosenAction], _mazeId));

        public RequestType RequestType => _type[_chosenAction];
    }
}