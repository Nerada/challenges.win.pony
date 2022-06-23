// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.MainWindowViewModel.cs
// Created on: 20210729
// -----------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json.Linq;
using Pony.Localization;
using Pony.Models;
using Pony.Rest;
using Pony.Support;

namespace Pony.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private readonly MazeParams    _mazeParams    = new();
        private readonly RestRequestor _restRequestor = new();

        private ICommand _clickCommand;
        private ICommand _walkCommand;
        private ICommand _changeLanguageCommand;

        private string _lastStatus; // Used for displaying status info
        private string _restStatus; // Used for displaying request to the server
        private string _mazeStatus; // Used for printing the maze

        private bool _validPlayerName = true;

        private string _startGameButtonContent = LocalizationHandler.GetString("MainWindowViewModel_button_start_game");

        private enum StatusType
        {
            Info,
            Warning,
            Error
        }

        private readonly Dictionary<StatusType, Brush> _statusColor = new()
        {
            { StatusType.Info, new SolidColorBrush(Colors.Black) },
            { StatusType.Warning, new SolidColorBrush(Colors.DarkOrange) },
            { StatusType.Error, new SolidColorBrush(Colors.Red) }
        };

        #endregion Fields

        #region Properties

        public static string WindowTitle         => LocalizationHandler.GetString("MainWindowViewModel_window_title");
        public static string LabelPlayerName     => LocalizationHandler.GetString("MainWindowViewModel_label_player_name");
        public static string LabelMazeWidth      => LocalizationHandler.GetString("MainWindowViewModel_label_maze_width");
        public static string LabelMazeHeight     => LocalizationHandler.GetString("MainWindowViewModel_label_maze_height");
        public static string LabelMazeDifficulty => LocalizationHandler.GetString("MainWindowViewModel_label_maze_difficulty");

        public string PlayerName
        {
            get => _mazeParams.PlayerName;
            set
            {
                _mazeParams.PlayerName = value;
                ValidPlayerName        = true;
                OnPropertyChange(nameof(PlayerName));
            }
        }

        public bool ValidPlayerName
        {
            get => _validPlayerName;
            set
            {
                _validPlayerName = value;
                OnPropertyChange(nameof(ValidPlayerName));
            }
        }

        public int MazeWidth
        {
            get => _mazeParams.Width;
            set
            {
                try
                {
                    _mazeParams.Width = value;
                    OnPropertyChange(nameof(MazeWidth));
                    SetStatus(StatusType.Info, string.Empty);
                }
                catch (InvalidInputException e)
                {
                    SetStatus(StatusType.Warning, e.Message);
                }

                OnPropertyChange(nameof(ValidMazeWidth));
            }
        }

        public bool ValidMazeWidth => _mazeParams.ValidWidth;

        public int MazeHeight
        {
            get => _mazeParams.Height;
            set
            {
                try
                {
                    _mazeParams.Height = value;
                    OnPropertyChange(nameof(MazeHeight));
                    SetStatus(StatusType.Info, string.Empty);
                }
                catch (InvalidInputException e)
                {
                    SetStatus(StatusType.Warning, e.Message);
                }

                OnPropertyChange(nameof(ValidMazeHeight));
            }
        }

        public bool ValidMazeHeight => _mazeParams.ValidHeight;

        public string MazeDifficulty
        {
            get => _mazeParams.Difficulty == null ? string.Empty : _mazeParams.Difficulty.ToString();
            set
            {
                try
                {
                    _mazeParams.Difficulty = string.IsNullOrEmpty(value) ? null : int.Parse(value, CultureInfo.InvariantCulture);

                    OnPropertyChange(nameof(MazeDifficulty));
                    SetStatus(StatusType.Info, string.Empty);
                }
                catch (InvalidInputException e)
                {
                    SetStatus(StatusType.Warning, e.Message);
                }
                catch (FormatException)
                {
                    SetStatus(StatusType.Warning, LocalizationHandler.GetString("MainWindowViewModel_wrong_difficulty"));
                }

                OnPropertyChange(nameof(ValidMazeDifficulty));
            }
        }

        public bool ValidMazeDifficulty => _mazeParams.ValidDifficulty;

        public static string RandomPlayer => MazeParams.GetRandomVerifiedPlayerName();

        public string Status
        {
            get => _lastStatus;
            set
            {
                _lastStatus = value;
                OnPropertyChange(nameof(Status));
                OnPropertyChange(nameof(StatusColor));
            }
        }

        public Brush StatusColor { get; set; }

        public string RestStatus
        {
            get => _restStatus;
            private set
            {
                _restStatus = value;
                OnPropertyChange(nameof(RestStatus));
            }
        }

        public string MazeStatus
        {
            get => _mazeStatus;
            private set
            {
                _mazeStatus = value;
                OnPropertyChange(nameof(MazeStatus));
            }
        }

        public string StartGameButtonContent
        {
            get => _startGameButtonContent;
            set
            {
                _startGameButtonContent = value;
                OnPropertyChange(nameof(StartGameButtonContent));
            }
        }

        public ICommand StartGameCommand => _clickCommand ??= new CommandHandler(_ => StartGameCmd(), () => CanExecuteStartGame);

        private bool CanExecuteStartGame
        {
            get
            {
                try
                {
                    _mazeParams.IsValid();
                    return true;
                }
                catch (InvalidInputException e)
                {
                    SetStatus(StatusType.Warning, e.Message);
                    return false;
                }
            }
        }

        public ICommand WalkCommand => _walkCommand ??= new CommandHandler(WalkCmd, () => CanExecuteWalk);

        private bool CanExecuteWalk { get; set; }

        public ICommand ChangeLanguageCommand => _changeLanguageCommand ??= new CommandHandler(ChangeLanguageCmd, () => CanExecuteChangeLanguage);

        private bool CanExecuteChangeLanguage { get; } = true;

        #endregion Properties

        #region Functions

        private void StartGameCmd()
        {
            if (!_mazeParams.IsValid()) return;

            RestStatus           = string.Empty;
            CanExecuteWalk       = false;
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                SetStatus(StatusType.Info, LocalizationHandler.GetString("MainWindowViewModel_info_setting_game"));
                JObject requestPayload = _mazeParams.ToJson();
                LogRestInfo(LocalizationHandler.GetString("MainWindowViewModel_rest_creating_maze"), requestPayload.ToString());
                LogRestInfo(LocalizationHandler.GetString("MainWindowViewModel_rest_created_maze"),  _restRequestor.CreateMaze(requestPayload));

                LogRestInfo(LocalizationHandler.GetString("MainWindowViewModel_rest_get_maze"));
                MazeStatus = _restRequestor.RetrieveMaze();

                CanExecuteWalk = true;

                StartGameButtonContent = LocalizationHandler.GetString("MainWindowViewModel_button_new_game");
                SetStatus(StatusType.Info, LocalizationHandler.GetString("MainWindowViewModel_info_ready_game"));
            }
            catch (InvalidPlayerNameException e)
            {
                ValidPlayerName = false;
                SetStatus(StatusType.Error, e.Message);
            }
            catch (Exception e)
            {
                SetStatus(StatusType.Error, e.Message);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void WalkCmd(object param)
        {
            Mouse.OverrideCursor = Cursors.Wait;

            try
            {
                LogRestInfo($"{LocalizationHandler.GetString("MainWindowViewModel_rest_walk_direction")} {(string)param}");

                SetStatus(StatusType.Info, _restRequestor.Move((string)param));

                LogRestInfo(LocalizationHandler.GetString("MainWindowViewModel_rest_updating_maze"));
                MazeStatus = _restRequestor.RetrieveMaze();
            }
            catch (Exception e)
            {
                LogRestInfo(e.ToString());
                SetStatus(StatusType.Error, e.Message);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ChangeLanguageCmd(object language)
        {
            if (Enum.TryParse((string)language, out LocalizationHandler.Language selectedLanguage))
            {
                LocalizationHandler.SetLanguage(selectedLanguage);

                // You can indicate all properties have changed by using null or string.Empty for the property name in PropertyChangedEventArgs.
                OnPropertyChange(string.Empty);
            }
        }

        private void SetStatus(StatusType type, string message)
        {
            StatusColor = _statusColor[type];
            Status      = message;
        }

        private void LogRestInfo(string action, string body = "")
        {
            string actionEnding = !string.IsNullOrEmpty(body) ? $": {Environment.NewLine}{body}" : ".";
            RestStatus = $"{DateTime.Now.ToString("HH:mm:ss", CultureInfo.InvariantCulture)} {action}{actionEnding}{Environment.NewLine}" +
                         RestStatus;
        }

        #endregion Functions

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Events
    }
}