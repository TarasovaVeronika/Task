using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using Task.Models;

namespace Task.Convert
{
    public class ConvertToJson : Converter
    {
        private readonly ILogger _logger;

        public ConvertToJson(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Выгружает информацию из сущности в .json формат.
        /// </summary>
        /// <param name="outputFile">Название выходного файла.</param>
        /// <param name="group">Сущность, из которой выгружаем информацию в файл.</param>
        public override void Convert(string outputFile, Group group)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                _logger.Information("Start writing to file.");

                using (StreamWriter streamWriter = new StreamWriter(outputFile))
                {
                    Newtonsoft.Json.JsonSerializerSettings jsonSerializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
                    streamWriter.Write(Newtonsoft.Json.JsonConvert.SerializeObject(group));
                }

            }
            catch (FileNotFoundException e)
            {
                _logger.Error($"{outputFile} - not found.\n" + e.Message);
                throw;
            }

            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            _logger.Information($"File's writing time: {elapsedTime}");
        }
    }
}
