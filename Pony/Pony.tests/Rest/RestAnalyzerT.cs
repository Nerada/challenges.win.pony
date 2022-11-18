// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.tests.RestAnalyzerT.cs
// Created on: 20210729
// -----------------------------------------------

using System;
using System.Net;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Pony.Models;
using Pony.Rest;
using Pony.Support;

namespace Pony.tests.Rest;

[TestClass]
public class RestAnalyzerT
{
    private readonly MazeParams    _params       = new();
    private readonly RestRequestor _restAnalyzer = new();

    [TestInitialize]
    public void Initializer()
    {
        _params.Height     = 20;
        _params.Width      = 20;
        _params.PlayerName = MazeParams.GetRandomVerifiedPlayerName();
    }

    [TestMethod]
    public void TestInvalidName()
    {
        _params.PlayerName = "Wrong Name";
        Action action = () => _restAnalyzer.CreateMaze(_params.ToJson());
        action.Should().Throw<InvalidPlayerNameException>();
    }

    [TestMethod]
    public void TestValidMazeCreation()
    {
        string result = _restAnalyzer.CreateMaze(_params.ToJson());

        JObject.Parse(result)["maze_id"].Should().NotBeNull();
    }

    [TestMethod]
    public void TestWrongMove()
    {
        _restAnalyzer.CreateMaze(_params.ToJson());

        Action action = () => _restAnalyzer.Move("Wrong Direction");
        action.Should().Throw<WebException>();
    }

    [TestMethod]
    public void TestWalking()
    {
        _restAnalyzer.CreateMaze(_params.ToJson());
        _restAnalyzer.Move("north").Should().BeOneOf("Move accepted", "Can't walk in there");
    }
}