using Challenge1.Models;
using Challenge1.Support;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Challenge1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private MazeParams _mazeParams = new MazeParams();
        private string _lastStatus;

        private ICommand _clickCommand;
        private ICommand _walkCommand;

        private RestHandler _restHandler = new RestHandler();
        private string _requestResponse;
        private string _mazeStatus;
        private string _startGameButtonContent = "Start game";
        private bool _readyToPlay;

        private enum StatusType { Info, Warning, Error }

        private Dictionary<StatusType, Brush> _statusColor = new Dictionary<StatusType, Brush>()
        {
            { StatusType.Info, new SolidColorBrush(Colors.Black) },
            { StatusType.Warning, new SolidColorBrush(Colors.DarkOrange) },
            { StatusType.Error, new SolidColorBrush(Colors.Red) }
        };

        #endregion Fields

        #region Properties

        public string PlayerName
        {
            get { return _mazeParams.PlayerName; }
            set
            {
                _mazeParams.PlayerName = value;
                OnPropertyChange(nameof(PlayerName));
            }
        }

        public int MazeWidth
        {
            get { return _mazeParams.Width; }
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

        public bool ValidMazeWidth { get { return _mazeParams.ValidWidth; } }

        public int MazeHeight
        {
            get { return _mazeParams.Height; }
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

        public bool ValidMazeHeight { get { return _mazeParams.ValidHeight; } }

        public string MazeDifficulty
        {
            get
            {
                return (_mazeParams.Difficulty == null) ? string.Empty : _mazeParams.Difficulty.ToString();
            }
            set
            {
                try
                {
                    if (string.IsNullOrEmpty(value)) { _mazeParams.Difficulty = null; }
                    else { _mazeParams.Difficulty = int.Parse(value); }

                    OnPropertyChange(nameof(MazeDifficulty));
                    SetStatus(StatusType.Info, string.Empty);
                }
                catch (InvalidInputException e)
                {
                    SetStatus(StatusType.Warning, e.Message);
                }
                catch (FormatException)
                {
                    SetStatus(StatusType.Warning, "Difficulty has to be a number.");
                }

                OnPropertyChange(nameof(ValidMazeDifficulty));
            }
        }

        public bool ValidMazeDifficulty { get { return _mazeParams.ValidDifficulty; } }

        public string RandomPlayer
        {
            get { return MazeParams.GetRandomVerifiedPlayerName(); }
        }

        public string Status
        {
            get { return _lastStatus; }
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
            get { return _requestResponse; }
            private set
            {
                _requestResponse = value;
                OnPropertyChange(nameof(RestStatus));
            }
        }

        public string MazeStatus
        {
            get { return _mazeStatus; }
            private set
            {
                _mazeStatus = value;
                OnPropertyChange(nameof(MazeStatus));
            }
        }

        public string StartGameButtonContent 
        { 
            get { return _startGameButtonContent; } 
            set
            {
                _startGameButtonContent = value;
                OnPropertyChange(nameof(StartGameButtonContent));
            }
        }

        public ICommand StartGameCommand => _clickCommand ?? (_clickCommand = new CommandHandler(param => StartGameCmd(param), () => CanExecuteStartGame));

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

        public ICommand WalkCommand => _walkCommand ?? (_walkCommand = new CommandHandler(param => WalkCmd(param), () => CanExecuteWalk));

        private bool CanExecuteWalk
        {
            get
            {
                return _readyToPlay;
            }
        }
        #endregion Properties

        #region Functions

        private void StartGameCmd(object param)
        {
            if (!_mazeParams.IsValid()) { return; }
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                JObject requestPayload = _mazeParams.ToJson();
                LogRestInfo("Creating maze", requestPayload.ToString());
                SetStatus(StatusType.Info, _restHandler.Request(RestHandler.Actions.CreateMaze, requestPayload));

                LogRestInfo("Retreiving the Maze");
                MazeStatus = _restHandler.Request(RestHandler.Actions.GetMaze);
                _readyToPlay = true;

                StartGameButtonContent = "Start new game";
            }
            catch (Exception e)
            {
                LogRestInfo(e.ToString());
                SetStatus(StatusType.Error, e.Message);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void WalkCmd(object param)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                JObject direction = new JObject();
                direction.Add("direction", (string)param);
                LogRestInfo("Walk in direction", direction.ToString());

                SetStatus(StatusType.Info, _restHandler.Request(RestHandler.Actions.NextMove, direction));

                LogRestInfo("Updating the Maze");
                MazeStatus = _restHandler.Request(RestHandler.Actions.GetMaze);
            }
            catch (Exception e)
            {
                LogRestInfo(e.ToString());
                SetStatus(StatusType.Error, e.Message);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void SetStatus(StatusType type, string message)
        {
            StatusColor = _statusColor[type];
            Status = message;
        }

        private void LogRestInfo(string action, string body = "")
        {
            string actionEnding = !string.IsNullOrEmpty(body) ? $": {Environment.NewLine}{body}" : ".";
            RestStatus = $"{DateTime.Now.ToString("HH:mm:ss")} {action}{actionEnding}{Environment.NewLine}{Environment.NewLine}" + RestStatus;
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