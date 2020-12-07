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
        private MainWindowViewModel _mainWindowVM;

        [TestInitialize]
        public void Initializer() { _mainWindowVM = new MainWindowViewModel(); }

        [TestMethod]
        public void TestWidthValidation()
        {
            _mainWindowVM.MazeWidth = 10;
            Assert.IsFalse(_mainWindowVM.ValidMazeWidth);

            _mainWindowVM.MazeWidth = 20;
            Assert.IsTrue(_mainWindowVM.ValidMazeWidth);
        }

        [TestMethod]
        public void TestHeightValidation()
        {
            _mainWindowVM.MazeHeight = 10;
            Assert.IsFalse(_mainWindowVM.ValidMazeHeight);

            _mainWindowVM.MazeHeight = 20;
            Assert.IsTrue(_mainWindowVM.ValidMazeHeight);
        }

        [TestMethod]
        public void TestDifficultyValidation()
        {
            _mainWindowVM.MazeDifficulty = "11";
            Assert.IsFalse(_mainWindowVM.ValidMazeDifficulty);

            _mainWindowVM.MazeDifficulty = "5";
            Assert.IsTrue(_mainWindowVM.ValidMazeDifficulty);
        }
    }
}