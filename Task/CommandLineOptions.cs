using MatthiWare.CommandLine.Core.Attributes;

namespace Task
{
    public class CommandLineOptions
    {
        [Required, Name("i", "InputFile"), Description("Input file name.")]
        public string InputFile { get; private set; }

        [Required, Name("o", "OutputFile"), Description("Output file name.")]
        public string OutputFile { get; set; }

        [Required, Name("f", "OutputFileFormat"), Description("Outut file format.")]
        public string OutputFileFormat { get; private set; }
    }
}
