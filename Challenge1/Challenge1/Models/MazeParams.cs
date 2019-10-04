using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge1
{
    class MazeParams
    {
        #region Fields

        private int                     m_width;
        private int                     m_height;
        private int?                    m_difficulty;
        private static readonly Random  m_random        = new Random();

        // Without google I would'nt have guessed even one of them, at least now the Pony part makes sense.
        // There could be more so lets use this list for random default names.
        // Too bad there is no "get valid name list" or "validate name" call to tbe API.
        public static List<string> ConfirmedNames = new List<string>()
        {
            "Apple Bloom", "Applejack", "Big McIntosh", "Derpy Hooves", "Fluttershy", "Pinkie Pie",
            "Princess Celestia", "Princess Luna", "Rainbow Dash", "Rarity", "Spike", "Sweetie Belle",
            "Twilight Sparkle"
        };

        #endregion Fields

        #region Properties
        
        public int Width
        {
            get { return m_width; }
            set
            {
                m_width = value;
                if (!IsValidSize(value)) { throw new InvalidInputException("Width is not between 15 and 25."); }
            }
        }

        public bool ValidWidth { get { return IsValidSize(Width); } }
        
        public int Height      
        {
            get { return m_height; }
            set 
            {
                m_height = value;
                if (!IsValidSize(value)) { throw new InvalidInputException("Height is not between 15 and 25."); }
            }
        }

        public bool ValidHeight { get { return IsValidSize(Height); } }

        public string PlayerName { get; set; } = GetRandomVerifiedPlayerName();

        public int? Difficulty
        {
            get { return m_difficulty; }
            set
            {
                m_difficulty = value;
                if (!IsValidDifficulty(value)) { throw new InvalidInputException("Difficulty is not between 0 and 10."); }
            }
        } 

        public bool ValidDifficulty { get { return IsValidDifficulty(Difficulty); } }

        #endregion Properties

        #region Functions

        public static string GetRandomVerifiedPlayerName()
        {
            return ConfirmedNames[m_random.Next(0, ConfirmedNames.Count - 1)];
        }

        public bool IsValid()
        {
            if (!IsValidSize(m_width)) { throw new InvalidInputException("Width is not between 15 and 25."); }
            if (!IsValidSize(m_height)) { throw new InvalidInputException("Height is not between 15 and 25."); }
            if (!IsValidDifficulty(m_difficulty)) { throw new InvalidInputException("Difficulty is not between 0 and 10."); }
            return true;
        }

        private bool IsValidSize(int size)
        {
            return (size >= 15 && size <= 25);
        }

        private bool IsValidDifficulty (int? dif)
        {
            return (dif == null || (dif >= 0 && dif <= 10));
        }

        #endregion Functions
    }
}
