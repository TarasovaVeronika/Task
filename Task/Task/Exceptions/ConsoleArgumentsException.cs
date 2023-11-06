using System;

namespace Task.Exceptions
{
    public class ConsoleArgumentsException : FormatException
    {
        public ConsoleArgumentsException(string message)
           : base(message) {; }

        public ConsoleArgumentsException()
            : base() {; }
    }
}
