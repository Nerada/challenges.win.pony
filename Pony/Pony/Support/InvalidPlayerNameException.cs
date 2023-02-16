// -----------------------------------------------
//     Author: Ramon Bollen
//      File: Pony.InvalidPlayerNameException.cs
// Created on: 20221119
// -----------------------------------------------

using System;

namespace Pony.Support;

public class InvalidPlayerNameException : Exception
{
    // ReSharper disable once UnusedMember.Global
    public InvalidPlayerNameException()
    {
    }

    public InvalidPlayerNameException(string message) : base(message)
    {
    }

    // ReSharper disable once UnusedMember.Global
    public InvalidPlayerNameException(string message, Exception inner) : base(message, inner)
    {
    }
}