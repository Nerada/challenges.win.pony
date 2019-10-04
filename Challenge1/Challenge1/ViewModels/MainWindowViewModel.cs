using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Challenge1
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields

        private MazeParams  m_mazeParams = new MazeParams();
        private string      m_lastStatus;

        #endregion Fields

        #region Properties

        public string PlayerName
        {
            get { return m_mazeParams.PlayerName; }
            set
            {
                m_mazeParams.PlayerName = value;
                OnPropertyChange(nameof(PlayerName));
            }
        }

        public int MazeWidth
        {
            get { return m_mazeParams.Width; }
            set
            {
                try
                {
                    m_mazeParams.Width = value;
                    OnPropertyChange(nameof(MazeWidth));
                    Status = string.Empty;
                }
                catch(InvalidInputException e)
                {
                     Status = e.Message;
                }

                OnPropertyChange(nameof(ValidMazeWidth));
            }
        }

        public bool ValidMazeWidth { get { return m_mazeParams.ValidWidth; } }

        public int MazeHeight
        {
            get { return m_mazeParams.Height; }
            set
            {
                try
                {
                    m_mazeParams.Height = value;
                    OnPropertyChange(nameof(MazeHeight));
                    Status = string.Empty;
                }
                catch (InvalidInputException e)
                {
                    Status = e.Message;
                }

                OnPropertyChange(nameof(ValidMazeHeight));
            }
        }

        public bool ValidMazeHeight { get { return m_mazeParams.ValidHeight; } }

        public string MazeDifficulty
        {
            get 
            {  
                return (m_mazeParams.Difficulty == null) ? string.Empty : m_mazeParams.Difficulty.ToString(); 
            }
            set
            {
                try
                {
                    if(value == string.Empty){ m_mazeParams.Difficulty = null; }
                    else { m_mazeParams.Difficulty = int.Parse(value); }

                    OnPropertyChange(nameof(MazeDifficulty));
                    Status = string.Empty;
                }
                catch (InvalidInputException e)
                {
                    Status = e.Message; 
                }
                catch (FormatException)
                {
                    Status = "Difficulty has to be a number.";
                }

                OnPropertyChange(nameof(ValidMazeDifficulty));
            }
        }

        public bool ValidMazeDifficulty { get { return m_mazeParams.ValidDifficulty; } }

        public string RandomPlayer
        {
            get { return MazeParams.GetRandomVerifiedPlayerName(); }
        }

        public string Status
        {
            get { return m_lastStatus; }
            set
            {
                m_lastStatus = value;
                OnPropertyChange(nameof(Status));
            }
        }

        #endregion Properties

        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChange(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Events
    }
}
