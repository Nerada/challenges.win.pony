using System;

namespace Challenge1.Support
{
    public class InvalidPlayerNameException : Exception
    {
        public InvalidPlayerNameException()
        {
        }

        public InvalidPlayerNameException(string message) : base(message)
        {
        }

        public InvalidPlayerNameException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}