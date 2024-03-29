﻿// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.RequestUrl.cs
// Created on: 20210729
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using static Pony.Rest.RestHandler;

namespace Pony.Rest;

public class RequestUrl
{
    public enum RestAction
    {
        CreateMaze,
        NextMove,
        GetMazeState,
        GetMaze
    }

    private readonly Dictionary<RestAction, RequestType> _type = new()
    {
        {RestAction.CreateMaze, RequestType.POST},
        {RestAction.GetMazeState, RequestType.GET},
        {RestAction.NextMove, RequestType.POST},
        {RestAction.GetMaze, RequestType.GET}
    };

    private readonly Dictionary<RestAction, string> _calls = new()
    {
        {RestAction.CreateMaze, "maze"},
        {RestAction.GetMazeState, "maze/{0}"},
        {RestAction.NextMove, "maze/{0}"},
        {RestAction.GetMaze, "maze/{0}/print"}
    };

    private readonly RestAction _chosenAction;

    private readonly string _mazeId;

    private readonly Uri _urlStart = new("https://ponychallenge.trustpilot.com/pony-challenge/");

    public RequestUrl(RestAction action, string mazeId = "")
    {
        if (action != RestAction.CreateMaze && string.IsNullOrEmpty(mazeId)) throw new Exception("Maze Id is required for this type of action.");

        _chosenAction = action;
        _mazeId       = mazeId;
    }

    public Uri Call => new(_urlStart, string.Format(CultureInfo.InvariantCulture, _calls[_chosenAction], _mazeId));

    public RequestType RequestType => _type[_chosenAction];
}