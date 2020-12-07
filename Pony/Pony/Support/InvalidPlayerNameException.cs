// -----------------------------------------------
//     Author: Ramon Bollen
//       File: Pony.InvalidPlayerNameException.cs
// Created on: 20191006
// -----------------------------------------------

using System;

namespace Pony.Support
{
    public class InvalidPlayerNameException : Exception
    {
        public InvalidPlayerNameException() { }

        public InvalidPlayerNameException(string message) : base(message) { }

        public InvalidPlayerNameException(string message, Exception inner) : base(message, inner) { }
    }
}