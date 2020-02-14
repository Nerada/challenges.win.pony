//-----------------------------------------------
//      Author: Ramon Bollen
//       File: Challenge1_Tests.MainWindowViewModelT.cs
// Created on: 2019108
//-----------------------------------------------

using Challenge1.ViewModels;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Challenge1_Tests
{
    [TestClass]
    public class MainWindowViewModelT
    {
        private MainWindowViewModel _mainWindowVM;

        [TestInitialize]
        public void Initializer()
        {
            _mainWindowVM = new MainWindowViewModel();
        }

        [TestMethod]
        public void TestWidthValidaton()
        {
            _mainWindowVM.MazeWidth = 10;
            Assert.IsFalse(_mainWindowVM.ValidMazeWidth);

            _mainWindowVM.MazeWidth = 20;
            Assert.IsTrue(_mainWindowVM.ValidMazeWidth);
        }

        [TestMethod]
        public void TestHeightValidaton()
        {
            _mainWindowVM.MazeHeight = 10;
            Assert.IsFalse(_mainWindowVM.ValidMazeHeight);

            _mainWindowVM.MazeHeight = 20;
            Assert.IsTrue(_mainWindowVM.ValidMazeHeight);
        }

        [TestMethod]
        public void TestDifficultyValidaton()
        {
            _mainWindowVM.MazeDifficulty = "11";
            Assert.IsFalse(_mainWindowVM.ValidMazeDifficulty);

            _mainWindowVM.MazeDifficulty = "5";
            Assert.IsTrue(_mainWindowVM.ValidMazeDifficulty);
        }
    }
}