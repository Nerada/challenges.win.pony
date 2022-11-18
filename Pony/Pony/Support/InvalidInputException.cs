// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.InvalidInputException.cs
// Created on: 20210729
// -----------------------------------------------

using System;

namespace Pony.Support;

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