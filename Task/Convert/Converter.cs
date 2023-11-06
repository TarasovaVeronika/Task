using Task.Models;

namespace Task.Convert
{
    public abstract class Converter
    {
        public abstract void Convert(string outputFile, Group group);
    }
}
