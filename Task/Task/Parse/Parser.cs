using MatthiWare.CommandLine;
using System;
using System.Linq;
using Task.Exceptions;

namespace Task.Parse
{
    internal class Parser
    {
        /// <summary>
        /// Проверяет параметры входной строки на корректность и в случае успеха возвращает true.
        /// </summary>
        /// <param name="args">Параметры входной строки.</param>
        /// <param name="programOptions">Класс, для хранения входных параметров.</param>
        /// <returns></returns>
        public bool Parse(string[] args, ref CommandLineOptions programOptions)
        {
            var parser = new CommandLineParser<CommandLineOptions>();
            var result = parser.Parse(args);

            if (result.HasErrors)
            {
                throw new ConsoleArgumentsException();
            }

            programOptions = result.Result;

            if (!Handling(programOptions))
            {
                throw new ConsoleArgumentsException("Invalid input.");
            }

            return true;
        }

        private bool Handling(CommandLineOptions programOptions)
        {
            if (!programOptions.InputFile.EndsWith(".csv"))
            {
                Console.WriteLine("|" + programOptions.InputFile + "|" +
                    "\n\nSupported input formats: \n\n" +
                    "\t\".csv\"");
                return false;
            }

            if (!programOptions.OutputFile.All(letter => Char.IsLetterOrDigit(letter)
                || letter.Equals('_')
                || programOptions.OutputFile.Any(space => Char.IsWhiteSpace(space))))
            {
                Console.WriteLine("|" + programOptions.OutputFile + "|" +
                    "- Invalid name for output file.");
                return false;
            }

            if (programOptions.OutputFileFormat == "Excel")
            {
                programOptions.OutputFile += ".xlsx";
            }
            else if (programOptions.OutputFileFormat == "JSON")
            {
                programOptions.OutputFile += ".json";
            }
            else
            {
                Console.WriteLine("|" + programOptions.OutputFileFormat + "|" +
                    "\n\nSupported output formats: \n\n" +
                    "\t\"Excel\"\n" +
                    "\t\"JSON\"\n");

                return false;
            }

            return true;
        }

    }
}