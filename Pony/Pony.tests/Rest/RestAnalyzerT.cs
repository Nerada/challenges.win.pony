// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.tests.RestAnalyzerT.cs
// Created on: 20201211
// -----------------------------------------------

using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Pony.Models;
using Pony.Rest;
using Pony.Support;

namespace Pony.tests.Rest
{
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
        [ExpectedException(typeof(InvalidPlayerNameException))]
        public void TestInvalidName()
        {
            _params.PlayerName = "Wrong Name";
            _restAnalyzer.CreateMaze(_params.ToJson());
        }

        [TestMethod]
        public void TestValidMazeCreation()
        {
            string result = _restAnalyzer.CreateMaze(_params.ToJson());

            Assert.IsNotNull(JObject.Parse(result)["maze_id"]);
        }

        [TestMethod]
        [ExpectedException(typeof(WebException))]
        public void TestWrongMove()
        {
            _restAnalyzer.CreateMaze(_params.ToJson());

            _restAnalyzer.Move("Wrong Direction");
        }

        [TestMethod]
        public void TestWalking()
        {
            _restAnalyzer.CreateMaze(_params.ToJson());

            string moveResponse = _restAnalyzer.Move("north");

            Assert.IsTrue(moveResponse == "Move accepted" || moveResponse == "Can't walk in there");
        }
    }
}