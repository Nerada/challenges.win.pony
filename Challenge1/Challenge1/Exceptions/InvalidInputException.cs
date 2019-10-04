using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge1
{
    class InvalidInputException : Exception
    {   
        public InvalidInputException() { }
        public InvalidInputException(string message) : base(message) { }
        public InvalidInputException(string message, Exception inner) : base(message, inner) { }
    }
}
