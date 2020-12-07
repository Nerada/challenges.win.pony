// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Pony.MainWindow.xaml.cs
// Created on: 20190927
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Pony.ViewModels;

namespace Pony
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly SolidColorBrush _black = new SolidColorBrush(Colors.Black);
        private readonly SolidColorBrush _gray  = new SolidColorBrush(Colors.Gray);

        private readonly Dictionary<TextBox, PreviousState> _previousValues = new Dictionary<TextBox, PreviousState>();

        public MainWindow()
        {
            InitializeComponent();
            var viewModel = new MainWindowViewModel();
            DataContext = viewModel;

            var textboxes = new List<TextBox> {TxtPlayerName, TxtWidth, TxtHeight, TxtDifficulty};
            textboxes.ForEach(tb =>
            {
                tb.Foreground = _gray;
                _previousValues.Add(tb, new PreviousState());

                tb.GotFocus  += DefaultTextBox_Enter;
                tb.LostFocus += DefaultTextBox_Leave;
            });
        }

        private void DefaultTextBox_Enter(object sender, EventArgs e)
        {
            var tb = (TextBox) sender;

            if (!string.IsNullOrEmpty(tb.Text))
            {
                _previousValues[tb].Text  = tb.Text;
                _previousValues[tb].Color = tb.Foreground;
            }

            tb.Text       = string.Empty;
            tb.Foreground = _black;
        }

        private void DefaultTextBox_Leave(object sender, EventArgs e)
        {
            var tb = (TextBox) sender;

            if (!string.IsNullOrEmpty(tb.Text.Trim())) return;

            // Lets get a new random player name just for fun
            if (tb == TxtPlayerName)
            {
                tb.Text       = MainWindowViewModel.RandomPlayer;
                tb.Foreground = _gray;
            }
            else if (tb != TxtDifficulty)
            {
                tb.Text       = _previousValues[tb].Text;
                tb.Foreground = _previousValues[tb].Color;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) => TxtRestInfo.Focus();

        private class PreviousState
        {
            public string Text  { get; set; }
            public Brush  Color { get; set; } = new SolidColorBrush(Colors.Gray);
        }
    }
}