using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Challenge1
{
    internal class MazeParams
    {
        #region Fields

        private int _width = 20;
        private int _height = 20;
        private int? _difficulty;
        private static readonly Random _random = new Random();

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
            get { return _width; }
            set
            {
                _width = value;
                if (!IsValidSize(value)) { throw new InvalidInputException("Width is not between 15 and 25."); }
            }
        }

        public bool ValidWidth { get { return IsValidSize(Width); } }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (!IsValidSize(value)) { throw new InvalidInputException("Height is not between 15 and 25."); }
            }
        }

        public bool ValidHeight { get { return IsValidSize(Height); } }

        public string PlayerName { get; set; } = GetRandomVerifiedPlayerName();

        public int? Difficulty
        {
            get { return _difficulty; }
            set
            {
                _difficulty = value;
                if (!IsValidDifficulty(value)) { throw new InvalidInputException("Difficulty is not between 0 and 10."); }
            }
        }

        public bool ValidDifficulty { get { return IsValidDifficulty(Difficulty); } }

        #endregion Properties

        #region Functions

        public static string GetRandomVerifiedPlayerName()
        {
            return ConfirmedNames[_random.Next(0, ConfirmedNames.Count - 1)];
        }

        public bool IsValid()
        {
            if (!IsValidSize(_width)) { throw new InvalidInputException("Width is not between 15 and 25."); }
            if (!IsValidSize(_height)) { throw new InvalidInputException("Height is not between 15 and 25."); }
            if (!IsValidDifficulty(_difficulty)) { throw new InvalidInputException("Difficulty is not between 0 and 10."); }
            return true;
        }

        private bool IsValidSize(int size)
        {
            return (size >= 15 && size <= 25);
        }

        private bool IsValidDifficulty(int? dif)
        {
            return (dif == null || (dif >= 0 && dif <= 10));
        }

        public JObject ToJson()
        {
            JObject returnJSON = new JObject(
                new JProperty("maze-width", Width),
                new JProperty("maze-height", Height),
                new JProperty("maze-player-name", PlayerName));
            if (Difficulty != null) { returnJSON.Add("difficulty", Difficulty); }
            return returnJSON;
        }

        #endregion Functions
    }
}