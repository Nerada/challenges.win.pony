// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Challenge1.InvalidPlayerNameException.cs
// Created on: 20191006
// -----------------------------------------------

using System;

namespace Challenge1.Support
{
    public class InvalidPlayerNameException : Exception
    {
        public InvalidPlayerNameException() { }

        public InvalidPlayerNameException(string message) : base(message) { }

        public InvalidPlayerNameException(string message, Exception inner) : base(message, inner) { }
    }
}