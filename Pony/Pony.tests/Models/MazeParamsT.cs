// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.tests.MazeParamsT.cs
// Created on: 20210729
// -----------------------------------------------

using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pony.Models;
using Pony.Support;

namespace Pony.tests.Models;

[TestClass]
public class MazeParamsT
{
    private MazeParams _mazeParams;

    [TestInitialize]
    public void Initializer() => _mazeParams = new MazeParams();

    [TestMethod]
    public void TestInvalidWidth()
    {
        Action action = () => _mazeParams.Width = 10;
        action.Should().Throw<InvalidInputException>();
    }

    [TestMethod]
    public void TestInvalidHeight()
    {
        Action action = () => _mazeParams.Height = 10;
        action.Should().Throw<InvalidInputException>();
    }

    [TestMethod]
    public void TestInvalidDifficulty()
    {
        Action action = () => _mazeParams.Difficulty = 11;
        action.Should().Throw<InvalidInputException>();
    }

    [TestMethod]
    public void TestParamsValidation()
    {
        _mazeParams.Height     = 20;
        _mazeParams.Width      = 20;
        _mazeParams.Difficulty = 5;
        _mazeParams.PlayerName = MazeParams.GetRandomVerifiedPlayerName();

        _mazeParams.IsValid().Should().BeTrue();
    }
}