// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Pony.tests.MainWindowViewModelT.cs
// Created on: 20191008
// -----------------------------------------------

using Pony.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Pony.tests.ViewModels
{
    [TestClass]
    public class MainWindowViewModelT
    {
        private MainWindowViewModel _mainWindowVm;

        [TestInitialize]
        public void Initializer() => _mainWindowVm = new MainWindowViewModel();

        [TestMethod]
        public void TestWidthValidation()
        {
            _mainWindowVm.MazeWidth = 10;
            Assert.IsFalse(_mainWindowVm.ValidMazeWidth);

            _mainWindowVm.MazeWidth = 20;
            Assert.IsTrue(_mainWindowVm.ValidMazeWidth);
        }

        [TestMethod]
        public void TestHeightValidation()
        {
            _mainWindowVm.MazeHeight = 10;
            Assert.IsFalse(_mainWindowVm.ValidMazeHeight);

            _mainWindowVm.MazeHeight = 20;
            Assert.IsTrue(_mainWindowVm.ValidMazeHeight);
        }

        [TestMethod]
        public void TestDifficultyValidation()
        {
            _mainWindowVm.MazeDifficulty = "11";
            Assert.IsFalse(_mainWindowVm.ValidMazeDifficulty);

            _mainWindowVm.MazeDifficulty = "5";
            Assert.IsTrue(_mainWindowVm.ValidMazeDifficulty);
        }
    }
}