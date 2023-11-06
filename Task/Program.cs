using Serilog;
using Serilog.Events;
using System;
using Task.Convert;
using Task.Exceptions;
using Task.Models;
using Task.Parse;
using Task.Unload;

namespace Task
{
    internal static class Program
    {

        private static ILogger _logger { get; set; }

        static Program()
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("-{Date}.txt", LogEventLevel.Information,
                     outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exceptions}")
                .CreateLogger();

        }

        public static void Main(string[] args)
        {
            Startup(args);
        }

        public static void Startup(string[] args)
        {

            Parser parser = new Parser();

            CommandLineOptions programOptions = new CommandLineOptions();

            try
            {

                parser.Parse(args, ref programOptions);

            }
            catch (ConsoleArgumentsException e)
            {

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;

                _logger.Error(e.Message);
                return;

            }

            Group group;

            UnloadToOjbect unloadToOjbect = new UnloadToOjbect(_logger);

            group = unloadToOjbect.Unload(programOptions.InputFile);

            if (programOptions.OutputFile.EndsWith(".xlsx"))
            {
                ConvertToExcel convertToExcel = new ConvertToExcel(_logger);
                convertToExcel.Convert(programOptions.OutputFile, programOptions.InputFile, group);
            }
            else
            {
                ConvertToJson convertToJson = new ConvertToJson(_logger);
                convertToJson.Convert(programOptions.OutputFile, group);
            }


        }
    }
}
