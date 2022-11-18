// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.tests.MainWindowViewModelT.cs
// Created on: 20210729
// -----------------------------------------------

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pony.ViewModels;

namespace Pony.tests.ViewModels;

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
        _mainWindowVm.ValidMazeWidth.Should().BeFalse();

        _mainWindowVm.MazeWidth = 20;
        _mainWindowVm.ValidMazeWidth.Should().BeTrue();
    }

    [TestMethod]
    public void TestHeightValidation()
    {
        _mainWindowVm.MazeHeight = 10;
        _mainWindowVm.ValidMazeHeight.Should().BeFalse();

        _mainWindowVm.MazeHeight = 20;
        _mainWindowVm.ValidMazeHeight.Should().BeTrue();
    }

    [TestMethod]
    public void TestDifficultyValidation()
    {
        _mainWindowVm.MazeDifficulty = "11";
        _mainWindowVm.ValidMazeDifficulty.Should().BeFalse();

        _mainWindowVm.MazeDifficulty = "5";
        _mainWindowVm.ValidMazeDifficulty.Should().BeTrue();
    }
}