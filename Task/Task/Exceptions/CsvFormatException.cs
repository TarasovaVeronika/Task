using System;

namespace Task.Exceptions
{
    public class CsvFormatException : FormatException
    {
        public CsvFormatException(string message)
           : base(message) {; }
    }
}
