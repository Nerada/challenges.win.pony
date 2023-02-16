// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.InvalidInputException.cs
// Created on: 20221119
// -----------------------------------------------

using System;

namespace Pony.Support;

public class InvalidInputException : Exception
{
    // ReSharper disable once UnusedMember.Global
    public InvalidInputException()
    {
    }

    public InvalidInputException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public InvalidInputException(string message, Exception inner) : base(message, inner)
    {
    }
}