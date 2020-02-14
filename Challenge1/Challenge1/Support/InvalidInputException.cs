//-----------------------------------------------
//      Author: Ramon Bollen
//       File: Challenge1.Support.InvalidInputException.cs
// Created on: 2019104
//-----------------------------------------------

using System;

namespace Challenge1.Support
{
    public class InvalidInputException : Exception
    {
        public InvalidInputException()
        {
        }

        public InvalidInputException(string message) : base(message)
        {
        }

        public InvalidInputException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}