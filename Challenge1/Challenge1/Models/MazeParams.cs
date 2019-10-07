using Challenge1.Resources;
using Challenge1.Support;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;

namespace Challenge1.Models
{
    /// <summary>
    /// Data model + validation for Maze parameters.
    /// </summary>
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
                if (!IsValidSize(value)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_width")); }
            }
        }

        public bool ValidWidth { get { return IsValidSize(Width); } }

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                if (!IsValidSize(value)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_height")); }
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
                if (!IsValidDifficulty(value)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_difficulty")); }
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
            if (!IsValidSize(_width)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_width")); }
            if (!IsValidSize(_height)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_height")); }
            if (!IsValidDifficulty(_difficulty)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_difficulty")); }
            return true;
        }

        private static bool IsValidSize(int size)
        {
            return (size >= 15 && size <= 25);
        }

        private static bool IsValidDifficulty(int? dif)
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