// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Pony.tests.MazeParamsT.cs
// Created on: 20191008
// -----------------------------------------------

using Pony.Models;
using Pony.Support;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pony.tests.Models
{
    [TestClass]
    public class MazeParamsT
    {
        private MazeParams _mazeParams;

        [TestInitialize]
        public void Initializer() { _mazeParams = new MazeParams(); }

        [TestMethod]
        [ExpectedException(typeof(InvalidInputException))]
        public void TestInvalidWidth() { _mazeParams.Width = 10; }

        [TestMethod]
        [ExpectedException(typeof(InvalidInputException))]
        public void TestInvalidHeight() { _mazeParams.Height = 10; }

        [TestMethod]
        [ExpectedException(typeof(InvalidInputException))]
        public void TestInvalidDifficulty() { _mazeParams.Difficulty = 11; }

        [TestMethod]
        public void TestParamsValidation()
        {
            _mazeParams.Height     = 20;
            _mazeParams.Width      = 20;
            _mazeParams.Difficulty = 5;
            _mazeParams.PlayerName = MazeParams.GetRandomVerifiedPlayerName();

            Assert.IsTrue(_mazeParams.IsValid());
        }
    }
}