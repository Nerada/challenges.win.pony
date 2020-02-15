// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Challenge1.MazeParams.cs
// Created on: 20191004
// -----------------------------------------------

using System;
using System.Collections.Generic;
using Challenge1.Resources;
using Challenge1.Support;
using Newtonsoft.Json.Linq;

namespace Challenge1.Models
{
    /// <summary>
    ///     Data model + validation for Maze parameters.
    /// </summary>
    public class MazeParams
    {
        #region Fields

        private                 int    _width  = 20;
        private                 int    _height = 20;
        private                 int?   _difficulty;
        private static readonly Random Random = new Random();

        #endregion Fields

        #region Properties

        // Without google I wouldn't have guessed any of them.
        // There could be more so lets use this list for random default names.
        // Too bad there is no "get valid name list" or "validate name" call to tbe API.
        public static List<string> ConfirmedNames =>
            new List<string> {
                "Apple Bloom",
                "Applejack",
                "Big McIntosh",
                "Derpy Hooves",
                "Fluttershy",
                "Pinkie Pie",
                "Princess Celestia",
                "Princess Luna",
                "Rainbow Dash",
                "Rarity",
                "Spike",
                "Sweetie Belle",
                "Twilight Sparkle"
            };

        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                if (!IsValidSize(value)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_width")); }
            }
        }

        public bool ValidWidth => IsValidSize(Width);

        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                if (!IsValidSize(value)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_height")); }
            }
        }

        public bool ValidHeight => IsValidSize(Height);

        public string PlayerName { get; set; } = GetRandomVerifiedPlayerName();

        public int? Difficulty
        {
            get => _difficulty;
            set
            {
                _difficulty = value;
                if (!IsValidDifficulty(value))
                {
                    throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_difficulty"));
                }
            }
        }

        public bool ValidDifficulty => IsValidDifficulty(Difficulty);

        #endregion Properties

        #region Functions

        public static string GetRandomVerifiedPlayerName() => ConfirmedNames[Random.Next(0, ConfirmedNames.Count - 1)];

        public bool IsValid()
        {
            if (!IsValidSize(_width)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_width")); }

            if (!IsValidSize(_height)) { throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_height")); }

            if (!IsValidDifficulty(_difficulty))
            {
                throw new InvalidInputException(ResourceHandler.GetString("MazeParams_wrong_difficulty"));
            }

            return true;
        }

        private static bool IsValidSize(int size) => size >= 15 && size <= 25;

        private static bool IsValidDifficulty(int? dif) => dif == null || (dif >= 0 && dif <= 10);

        public JObject ToJson()
        {
            var returnJSON = new JObject(new JProperty("maze-width", Width),
                                         new JProperty("maze-height", Height),
                                         new JProperty("maze-player-name", PlayerName));
            if (Difficulty != null) { returnJSON.Add("difficulty", Difficulty); }

            return returnJSON;
        }

        #endregion Functions
    }
}